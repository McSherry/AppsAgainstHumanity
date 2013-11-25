using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game
{
    public class Game
    {
        /// <summary>
        /// The port an AAH server runs on. Selected because it's an easy to remember
        /// sequence (being nearly linear AND fibonacci) and because it isn't something
        /// likely to be already taken.
        /// </summary>
        private const int PORT = 11235;
        /// <summary>
        /// The delimiter used in server messages.
        /// </summary>
        private const byte ETX = 3;

        private Random _RNG;
        private ulong _gameSeed;
        private bool _gameWon = false;
        private bool _hasStarted = false;
        private NetLibServer _server;
        private AAHProtocolWrapper _serverWrapper;

        /// <summary>
        /// Determines whether the given nickname meets the requirements.
        /// Maximum length 20 chars, alphanumeric, _, |
        /// </summary>
        /// <param name="nick">The nickname to determine the validity of.</param>
        /// <returns>Whether the nickname is valid.</returns>
        private bool _validNick(string nick)
        {
            char[] validNickChars = 
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_|".ToCharArray();

            bool hasEncounteredInvalid = false;
            foreach (char c in nick)
                hasEncounteredInvalid = !validNickChars.Contains(c) ? true : hasEncounteredInvalid;

            if (hasEncounteredInvalid || nick.Length > 20) return false;
            else if (nick.Length <= 20 && !hasEncounteredInvalid) return true;
            else return false;
        }
        /// <summary>
        /// Determines whether the provided nickname is free within the current game.
        /// </summary>
        /// <param name="nick">The nickname to determine the availability of.</param>
        /// <returns>Whether the given nickname is available.</returns>
        private bool _freeNick(string nick)
        {
            bool dupeNickEncountered = false;
            foreach (Player p in Players)
                dupeNickEncountered = p.Nickname.ToLower() == nick.ToLower() ? true : dupeNickEncountered;
            return !dupeNickEncountered;
        }

        // handles any JOIN commands received by the server.
        private void _handlerJOIN(long sender, string[] args)
        {
            // Game hasn't started, so we can allow the player to join,
            // assuming the player limit has not yet been reached.
            if (!_hasStarted)
            {
                // If the length of this class's property Players (number of connected players)
                // is equal to the maximum number specified in the parameters, refuse the connection
                // with the limit-reached message.
                if (this.Players.Count == this.Parameters.Players)
                    _serverWrapper.SendCommand(
                        CommandType.REFU,
                        new string[1] { "Player limit reached." },
                        sender
                        );
                else
                {
                    if (_validNick(args[0]) && _freeNick(args[0]))
                    {
                        // If the nickname is valid, add the player to the list of players
                        // and send a nickname-accept (NACC) to the client.
                        Player newPlayer = new Player(args[0], sender);
                        this.Players.Add(newPlayer);
                        _serverWrapper.SendCommand(CommandType.NACC, (string)null, sender);
                        _senderCLNF(sender);
                        _senderCLJN(sender, newPlayer);
                    }
                    else if (!_validNick(args[0]))
                    {
                        // If the nickname is invalid, send NDNY with appropriate text.
                        _serverWrapper.SendCommand(CommandType.NDNY, "Invalid nickname.", sender);
                    }
                    else if (!_freeNick(args[0]))
                    {
                        // If the nickname is already in use, send NDNY with appropriate text.
                        _serverWrapper.SendCommand(CommandType.NDNY, "Nickname in use.", sender);
                    }
                    // If nickname is neither free nor valid, send NDNY.
                    else _serverWrapper.SendCommand(CommandType.NDNY, "Nickname refused.", sender);
                }
            }
            else
            {
                // The game has already started, so we'll disallow clients
                // from joining. This is purely to simplify matters. We could
                // implement game-in-progress joining, but it's far simpler to
                // just refuse connection attempts.
                _serverWrapper.SendCommand(
                    CommandType.REFU,
                    "Game in progress; joining prohibited.",
                    sender
                    );
            }
        }

        // sends CLNFs to a client 
        private void _senderCLNF(long clientID)
        {
            // This sends the connecting client its own nickname,
            // so clients should probably compensate for this if
            // necessary.
            string[] playerNames = new string[this.Players.Count];
            int ctr = 0;
            if (playerNames.Length > 1)
            {
                foreach (Player p in Players)
                {
                    playerNames[ctr] = p.Nickname;
                    ++ctr;
                }

                _serverWrapper.SendCommand(CommandType.CLNF, playerNames, clientID);
            }
        }
        // sends CLJN (Client Join) to other clients, informing them
        // that another player has joined the game.
        private void _senderCLJN(long joinedClientID, Player joinedPlayer)
        {
            foreach (Player p in Players)
            {
                if (p.ClientIdentifier == joinedClientID) continue;
                _serverWrapper.SendCommand(CommandType.CLJN, joinedPlayer.Nickname, p.ClientIdentifier);
            }
        }

        /// <summary>
        /// Selects a black card from the pool, and removes it from the pool.
        /// </summary>
        /// <returns>The selected black card.</returns>
        private BlackCard _selectBlack()
        {
            int index = _RNG.Next(BlackCardPool.Count);
            BlackCard choice = BlackCardPool[index];
            BlackCardPool.RemoveAt(index);

            return choice;
        }
        /// <summary>
        /// Selects the specified number of white cards from the pool.
        /// </summary>
        /// <param name="n">The number of white cards to select.</param>
        /// <returns>The white cards selected, with their IDs.</returns>
        private Dictionary<int, WhiteCard> _selectWhites(int n)
        {
            Dictionary<int, WhiteCard> choices = new Dictionary<int, WhiteCard>();

            for (int i = 0; i < n; i++)
            {
                KeyValuePair<int, WhiteCard> choice = WhiteCardPool.ElementAt(_RNG.Next(WhiteCardPool.Count));
                WhiteCardPool.Remove(choice.Key);
                choices.Add(choice.Key, choice.Value);
            }

            return choices;
        }

        public Game(GameParameters gameParams)
        {
            this.Parameters = gameParams;
            this._gameSeed = new Crypto.SRand();
            this._RNG = new Random((int)_gameSeed);
            this.WhiteCardPool = new Dictionary<int, WhiteCard>();
            this.BlackCardPool = new List<BlackCard>();
            this.Players = new List<Player>();

            foreach (WhiteCard wc in Parameters.Cards.WhiteCards)
            {
                this.WhiteCardPool.Add(_RNG.Next(), wc);
            }
            this.BlackCardPool = Parameters.Cards.BlackCards;


            this._server = new NetLibServer(PORT, TransferProtocol.Delimited);
            _server.Delimiter = ETX;
            this._serverWrapper = new AAHProtocolWrapper(_server);

            _serverWrapper.RegisterCommandHandler(CommandType.JOIN, _handlerJOIN);

            _server.Start();
        }

        /// <summary>
        /// The parameters the game is currently configured to use.
        /// </summary>
        public GameParameters Parameters { get; private set; }
        /// <summary>
        /// A dictionary of white cards remaining in the pool, with IDs as their key.
        /// </summary>
        public Dictionary<int, WhiteCard> WhiteCardPool { get; private set; }
        /// <summary>
        /// A list of black cards remaining in the pool. Black cards do not have IDs.
        /// </summary>
        public List<BlackCard> BlackCardPool { get; private set; }
        /// <summary>
        /// The players currently in the game, with internal NetLib identifiers.
        /// </summary>
        public List<Player> Players { get; private set; }
        /// <summary>
        /// The white cards currently drawn by each player.
        /// </summary>
        public Dictionary<Player, Dictionary<int, WhiteCard>> DrawnCards { get; internal set; }

        public void Start()
        {
            if (!_hasStarted) _hasStarted = true;
            else throw new Exception("This game has already been started. Please create a new instance to start a new game.");

            while (!_gameWon)
            {
                BlackCard roundBlack = _selectBlack();
                Dictionary<int, WhiteCard> roundPool = _selectWhites(roundBlack.Pick * Players.Count);
                Round round = new Round(roundBlack, roundPool, Players, this);

                Player roundWinner = round.Start();
                ++roundWinner.AwesomePoints;
                // TODO: Verify the above works.
                // Dunno if returning a player maintains the pass by reference shit
                // which would allow modification of that player's class.
            }
        }
    }
}

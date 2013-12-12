using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private System.Timers.Timer _pingTimer;
        private Round _currRound;
        private Thread _gameThread;

        // use instead of AAHProtocolWrapper.SendCommand()
        // handles when SendCommand() returns false (e.g. player not available)
        // and removes player from dicts/lists
        internal void SendCommand(CommandType type, string[] args = null, long clientID = 0)
        {
            if (args == null) args = new string[1] { String.Empty };
            if (!_serverWrapper.SendCommand(type, args, clientID))
            {
                try
                {
                    Player remPlayer = Players.First(pl => pl.ClientIdentifier == clientID);
                    Players.Remove(remPlayer);
                    // Close the connection after refusing.
                    //_server.Clients[clientID].TcpClient.Close();
                    // Remove client after disconnecting
                    //_server.Clients.Remove(clientID);
                    // TODO: Uncomment these before production!
                    // Uncommented for debugging prior to cards being drawn.
                    //DrawnCards.Remove(remPlayer);
                    //_currRound.End();

                    foreach (Player p in Players.ToList())
                        _senderCLNF(p.ClientIdentifier);
                }
                // Players.First() found no match, probably because there are no players
                // we'll ignore it and continue.
                catch (InvalidOperationException) { }
            }
        }
        internal void SendCommand(CommandType type, string arg = null, long clientID = 0)
        {
            SendCommand(
                type,
                (arg == null ? new string[1] { String.Empty } : new string[1] { arg }),
                clientID
                );
        }

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
            foreach (Player p in Players.ToList())
                dupeNickEncountered = p.Nickname.ToLower() == nick.ToLower() ? true : dupeNickEncountered;
            return !dupeNickEncountered;
        }

        // handles any JOIN commands received by the server.
        // TODO: RESPOND WITH ACKN NOT NACC YOU IDIOT
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
                {
                    SendCommand(
                        CommandType.REFU,
                        new string[1] { "Player limit reached." },
                        sender
                        );
                    // Close the connection after refusing.
                    _server.Clients[sender].TcpClient.Close();
                    // Remove client after disconnecting
                    _server.Clients.Remove(sender);
                }
                else
                {
                    if (_validNick(args[0]) && _freeNick(args[0]))
                    {
                        // If the nickname is valid, add the player to the list of players
                        // and send a nickname-accept (NACC) to the client.
                        Player newPlayer = new Player(args[0], sender);
                        this.Players.Add(newPlayer);
                        SendCommand(CommandType.ACKN, (string[])null, sender);
                        _senderCLNF(sender);
                        _senderCLJN(sender, newPlayer);

                        if (this.OnPlayerJoin != null) this.OnPlayerJoin.Invoke(newPlayer);
                    }
                    else if (!_validNick(args[0]))
                    {
                        // If the nickname is invalid, send NDNY with appropriate text.
                        SendCommand(CommandType.REFU, "Nickname contains invalid characters.", sender);
                        // Close the connection after refusing.
                        _server.Clients[sender].TcpClient.Close();
                        // Remove client after disconnecting
                        _server.Clients.Remove(sender);
                    }
                    else if (!_freeNick(args[0]))
                    {
                        // If the nickname is already in use, send NDNY with appropriate text.
                        SendCommand(CommandType.REFU, "Nickname in use.", sender);
                        // Close the connection after refusing.
                        _server.Clients[sender].TcpClient.Close();
                        // Remove client after disconnecting
                        _server.Clients.Remove(sender);
                    }
                    // If nickname is neither free nor valid, send NDNY.
                    else
                    {
                        SendCommand(CommandType.REFU, "Nickname refused.", sender);
                        // Close the connection after refusing.
                        _server.Clients[sender].TcpClient.Close();
                        // Remove client after disconnecting
                        _server.Clients.Remove(sender);
                    }
                }
            }
            else
            {
                // The game has already started, so we'll disallow clients
                // from joining. This is purely to simplify matters. We could
                // implement game-in-progress joining, but it's far simpler to
                // just refuse connection attempts.
                SendCommand(
                    CommandType.REFU,
                    "Game in progress; joining prohibited.",
                    sender
                    );
                // Close the connection after refusing.
                _server.Clients[sender].TcpClient.Close();
                // Remove client after disconnecting
                _server.Clients.Remove(sender);
            }
        }
        // handles NICK requests from clients.
        private void _handlerNICK(long sender, string[] args)
        {
            if (_validNick(args[0]) && _freeNick(args[0]))
            {
                // LINQ is much nicer than a foreach loop with a condition in it, no?
                // TODO: Account for when a NICK is sent after a JOIN was denied.
                Player p = Players.First(pn => pn.ClientIdentifier == sender);
                // We're only expecting one player with the ID, so first element
                // will be fine.
                SendCommand(CommandType.NACC, "Nickname changed successfully.", sender);
                foreach (Player py in Players.ToList())
                {
                    if (py != p)
                    {
                        _senderCLNF(py.ClientIdentifier);
                    }
                }
            }
            else if (!_validNick(args[0]))
            {
                SendCommand(CommandType.NDNY, "Nickname invalid; unchanged.", sender);
            }
            else if (!_freeNick(args[0]))
            {
                SendCommand(CommandType.NDNY, "Nickname in use; unchanged.", sender);
            }
            else
            {
                SendCommand(CommandType.NDNY, "Nickname refused.", sender);
            }
        }
        // performs tasks required when a client leaves
        private void _handlerClientLeave(long clientID)
        {
            Player p = Players.First(pl => pl.ClientIdentifier == clientID);
            Players.Remove(p);
            DrawnCards.Remove(p);

            foreach (Player pl in Players.ToList())
            {
                _senderCLEX(pl.ClientIdentifier, p);
            }

            if (this.OnPlayerLeave != null) this.OnPlayerLeave.Invoke(p);
        }
        // handles SMSG requests received by the server
        // these are chat requests, the server acts as a relay
        // between clients
        private void _handlerSMSG(long sender, string[] args)
        {
            Player p = Players.First(pl => pl.ClientIdentifier == sender);
            _senderRMSG(p, args[0]);
        }
        // handles PICKs from clients + fires event
        private void _handlerPICK(long sender, string[] args)
        {
            List<int> cardIds = new List<int>();

            foreach (string s in args)
            {
                int cid = 0;
                if (int.TryParse(s, out cid)) cardIds.Add(cid);
                else
                {
                    SendCommand(
                        CommandType.UNRG, 
                        String.Format(
                            "Malformed Card Identifier {0}.",
                            s),
                        sender
                    );
                }
            }

            if (cardIds.Count > 0)
            {
                if (OnPlayerPick != null) OnPlayerPick
                    .Invoke(
                        Players.First(pl => pl.ClientIdentifier == sender),
                        cardIds.ToArray()
                        );
            }
        }
        // handles CZPKs from clients + fires event
        private void _handlerCZPK(long sender, string[] args)
        {
            int cardId = 0;

            // If the Card ID we received from CZPK is valid,
            // fire the event handler and transfer control to
            // whatever has attached itself to this handler.
            if (int.TryParse(args[0], out cardId))
            {
                if (OnCzarPick != null) OnCzarPick
                    .Invoke(
                        Players.First(pl => pl.ClientIdentifier == sender),
                        cardId
                    );
            }
            // If the ID isn't valid, response to the client which
            // sent it with an UNRG and drop the CZPK without
            // firing the event handler.
            else
            {
                SendCommand(
                    CommandType.UNRG,
                    "Card identifier was malformed.",
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
            foreach (Player p in Players.ToList())
            {
                playerNames[ctr] = p.Nickname;
                ++ctr;
            }

            SendCommand(CommandType.CLNF, playerNames, clientID);
        }
        // sends CLJN (Client Join) to other clients, informing them
        // that another player has joined the game.
        private void _senderCLJN(long joinedClientID, Player joinedPlayer)
        {
            foreach (Player p in Players.ToList())
            {
                if (p.ClientIdentifier == joinedClientID) continue;
                SendCommand(CommandType.CLJN, joinedPlayer.Nickname, p.ClientIdentifier);
            }
        }
        // Sends CLEX to clients informing them another client has exited
        // the game
        private void _senderCLEX(long clientID, Player exitedPlayer)
        {
            SendCommand(CommandType.CLEX, exitedPlayer.Nickname, clientID);
        }
        // relays the contents of SMSG to other clients in the form
        // of an RMSG
        private void _senderRMSG(Player sender, string message)
        {
            if (message.Length > 200)
            {
                // If message length is over 200 chars, return an error (UNRG) to the client which
                // send the message.
                SendCommand(CommandType.UNRG, "Messages have a 200-character limit.", sender.ClientIdentifier);
                return;
            }

            this.OnClientMessageReceived.Invoke(sender, message);
            foreach (Player p in Players.ToList())
            {
                SendCommand(CommandType.RMSG, new string[2] { sender.Nickname, message }, p.ClientIdentifier);
            }
        }
        // Send a broadcast from the server to all clients. No character
        // limit is enforced here.
        private void _senderBDCS(string message)
        {
            // ensure there are actually players to send the broadcast to
            if (Players.Count > 0)
            {
                foreach (Player p in Players.ToList())
                {
                    SendCommand(CommandType.BDCS, new string[1] { message }, p.ClientIdentifier);
                }
            }
            else return;
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
            this.DrawnCards = new Dictionary<Player, Dictionary<int, WhiteCard>>();

            foreach (WhiteCard wc in Parameters.Cards.WhiteCards)
            {
                this.WhiteCardPool.Add(_RNG.Next(), wc);
            }
            this.BlackCardPool = Parameters.Cards.BlackCards;


            _pingTimer = new System.Timers.Timer(5000);
            _pingTimer.Elapsed += (s, e) =>
            {
                // Sends a PING to each player.
                // As SendCommand wraps over the actual function and handles removals,
                // a PING which fails to deliver will be automatically handed by the
                // method and will have the player removed.
                foreach (Player p in Players.ToList())
                    SendCommand(CommandType.PING, (string[])null, p.ClientIdentifier);
            };
            // TODO: Re-enable this at production!
            // Pings disabled because they spam the console
            // and this is annoying during debugging.
            //_pingTimer.Start();


            this._server = new NetLibServer(PORT, TransferProtocols.Delimited, Encoding.ASCII);
            _server.Delimiter = ETX;
            this._serverWrapper = new AAHProtocolWrapper(_server);

            _serverWrapper.RegisterCommandHandler(CommandType.JOIN, _handlerJOIN);
            _serverWrapper.RegisterCommandHandler(CommandType.NICK, _handlerNICK);
            _serverWrapper.RegisterCommandHandler(CommandType.SMSG, _handlerSMSG);
            _serverWrapper.RegisterCommandHandler(CommandType.PICK, _handlerPICK);
            _serverWrapper.RegisterCommandHandler(CommandType.CZPK, _handlerCZPK);

            _server.StartListening();
        }

        /// <summary>
        /// Broadcasts a message from the server to all players.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        public void Broadcast(string message)
        {
            _senderBDCS(message);
        }

        public delegate void ClientMessageEventHandler(Player sender, string message);
        public delegate void PlayerEventHandler(Player p);
        public delegate void PlayerCardsEventHandler(Player p, int[] cardIDs);
        public delegate void PlayerCardEventHandler(Player p, int cardID);
        /// <summary>
        /// Fired when a valid message is received from a client.
        /// </summary>
        public event ClientMessageEventHandler OnClientMessageReceived;
        /// <summary>
        /// Fired when a player joins the game.
        /// </summary>
        public event PlayerEventHandler OnPlayerJoin;
        /// <summary>
        /// Fired when a player leaves the game.
        /// </summary>
        public event PlayerEventHandler OnPlayerLeave;
        /// <summary>
        /// Fired when a player picks a card. No checks as to whether a round is
        /// current in play, only criterion is the receival of a PICK command.
        /// </summary>
        public event PlayerCardsEventHandler OnPlayerPick;
        /// <summary>
        /// Fired when a CZPK is received.
        /// </summary>
        public event PlayerCardEventHandler OnCzarPick;

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
        /// <summary>
        /// Indicates whether the game has started.
        /// </summary>
        public bool HasStarted { get { return _hasStarted; } }

        public void Start()
        {
            if (!_hasStarted) _hasStarted = true;
            else throw new Exception("This game has already been started. Please create a new instance to start a new game.");

            _gameThread = new Thread(() =>
            {
                foreach (Player p in Players.ToList())
                    // Indicates that the game has started.
                    SendCommand(CommandType.GSTR, (string[])null, p.ClientIdentifier);

                // Used to keep track of the Card Czar. A random number at the start of the round,
                // then incremented by one (mod number of players) in each following round.
                int czarCtr = _RNG.Next(Players.Count);
                foreach (Player p in Players.ToList())
                {
                    // Draw 10 white cards per player, and send them to the player. These are the
                    // 10 cards drawn at the beginning of a game.
                    DrawnCards.Add(p, _selectWhites(10));
                    foreach (KeyValuePair<int, WhiteCard> card in DrawnCards[p].ToList())
                        SendCommand(
                            CommandType.WHTE,
                            new string[2] { card.Key.ToString(), card.Value.Text.ToString() },
                            p.ClientIdentifier
                        );
                }
                // Game loop, which will terminate once the Awesome Point limit for a single player
                // is reached.
                while (!_gameWon)
                {

                    BlackCard roundBlack = _selectBlack();
                    // The cards which will be given to players at the start of this round.
                    // Does not include the ten cards drawn at the start of a game.
                    Dictionary<int, WhiteCard> roundPool = _selectWhites(roundBlack.Draw * Players.Count);
                     _currRound = new Round(roundBlack, roundPool, this, Players[czarCtr]);

                    Player roundWinner = _currRound.Start();
                    if (roundWinner == null)
                    {
                        // Do something to indicate to other players that the
                        // Card Czar didn't pick within the alloted time.
                    }
                    else
                    {
                        ++roundWinner.AwesomePoints;
                    }

                    // Check whether a player has the number of points required to win the game.
                    // This will result in maybeWinner being NULL if no such player exists.
                    Player maybeWinner = Players.FirstOrDefault(pl => pl.AwesomePoints == Parameters.PointsLimit);
                    if (maybeWinner != null)
                    {
                        // We've got a winner, so we no longer require this game loop.
                        // Setting _gameWon to true will end the loop after this
                        // iteration finishes.
                        _gameWon = true;

                        foreach (Player p in Players.ToList())
                        {
                            // Inform each player of the winner of the current game.
                            SendCommand(
                                CommandType.GWIN,
                                maybeWinner.Nickname,
                                p.ClientIdentifier
                            );
                            // Request that clients gracefully disconnect.
                            SendCommand(
                                CommandType.DISC,
                                (string[])null,
                                p.ClientIdentifier
                            );
                        }

                        // Give clients a 2.5s grace period within which to disconnect.
                        Thread.Sleep(2500);
                        // After the grace period is up, we'll forcefully kill the
                        // thread. The exact method may change in future, but this
                        // is what we're doing for now.
                        _gameThread.Abort();
                    }

                    // Cause a 5000ms (5s) delay before the beginning of the next round.
                    // Before the delay, inform each player of the delay via a broadcast.
                    foreach (Player p in Players.ToList())
                    {
                        SendCommand(
                            CommandType.BDCS,
                            "The next round will begin in 5 seconds.",
                            p.ClientIdentifier
                        );
                    }
                    Thread.Sleep(5000);


                    // Increments the Card Czar counter by one, rolling over to zero if the number goes
                    // above the number of players. This causes Czars to be chosen sequentially. Since the
                    // counter is out of loop, the foreach at the start of the loop will choose the player
                    // indicated by the counter at the point below this comment.
                    // TODO: Uncomment this!
                    czarCtr = ++czarCtr % Players.Count;
                }

                // Stop pinging, as the game is over now
                _pingTimer.Stop();
            });

            _gameThread.Start();
        }
        public void Stop(bool stopServer = false)
        {
            if (!stopServer)
            {
                _gameThread.Abort();

                foreach (Player p in Players.ToList())
                {
                    SendCommand(CommandType.DISC, "Game ended by server administrator.", p.ClientIdentifier);
                }
            }
            else
            {
                if (_hasStarted) Stop();
                // TODO: FIX EXCEPTION WITH THIS
                //_server.Stop();
            }
        }
    }
}

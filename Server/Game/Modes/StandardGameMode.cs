using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsNetLib2;
using System.Threading;
using System.Timers;

namespace AppsAgainstHumanity.Server.Game.Modes
{
    public class StandardGameMode : GameMode
    {
        /// <summary>
        /// The player to be returned as the winner of the round.
        /// </summary>
        private Player _roundWinner = null;
        /// <summary>
        /// The thread on which the game-mode runs.
        /// </summary>
        private Thread _gmThread;

        public StandardGameMode(Game parent)
            : base(parent)
        {

        }

        public override string Name
        {
            get { return "Standard"; }
        }

        // This method will be called on a new thread by the Game
        // or Round parent class, so we won't do any thread buggery
        // in here.
        public override void Start()
        {
            this._gmThread = new Thread(() =>
            {
                // The timer we'll use to provide a period in which PICK commands will
                // be accepted and processed.
                var pickTimer = new System.Timers.Timer(base.Parent.Parameters.TimeoutLimit * 1000);
                bool pickTimerHasElapsed = false;
                pickTimer.Elapsed += (s, e) =>
                {
                    // Select all players that have not played the correct number of cards. Since
                    // the time has now elapsed, these players will be skipped in this round, and
                    // their cards returned.
                    foreach (Player p in (from pl in base.PickedList where !pl.Value select pl.Key))
                    {
                        // Inform the client that they timed out and have been skipped.
                        base.Parent.SendCommand(
                            CommandType.CRTO,
                            (string[])null,
                            p.ClientIdentifier
                        );
                        // Inform other players that this player timed out and has been skipped because of
                        // this. No need to say whether they were disconnected for timing out, as a call to
                        // Disconnect() below will send the relevant information to clients to allow them
                        // to show this message.
                        base.Parent.Broadcast(
                            String.Format(
                                "Player {0} failed to make a choice within the time-limit and has been skipped.",
                                p.Nickname
                            )
                        );
                        // If the server is configured to kick players when they time out,
                        // then we'll use the Game.Disconnect() method. This probably does
                        // some important stuff along with sending a DISC, but I'm not
                        // looking at the file right now and can't remember, so yeah.
                        if (base.Parent.Parameters.KickOnTimeout)
                            base.Parent.Disconnect(p.Nickname, "Disconnected due to inactivity.");
                    }

                    // Set the variable indicating whether the timer has elapsed to true,
                    // ending the loop waiting for players to submit picks and allowing the
                    // thread to continue.
                    pickTimerHasElapsed = true;
                };

                // Start the timer
                pickTimer.Start();
                // Allow PICKs to be received and processed
                base.AllowPicks = true;
                // Block the current thread, as the receiving and processing of
                // PICKs is performed on another thread. Blocking this thread means
                // that the game cannot progress.
                while (base.PickedList.ContainsValue(false) ^ pickTimerHasElapsed) ;
                // Stop allowing the receival and processing of PICKs.
                base.AllowPicks = false;
                // Stop the timer.
                pickTimer.Stop();
                // Dispose the timer's resources; we won't be using it again.
                pickTimer.Dispose();

                // We check whether the number of players that have successfully played this
                // round is greater than or equal to the minimum number of players required
                // to play, subtracting one to account for the Card Czar.
                if (base.PickedList.Where(pl => pl.Value).Count() >= (Constants.MinimumPlayers - 1))
                {
                    // If the minimum number of players have played, we'll continue with the round
                    // and reveal cards to other players and the Card Czar.

                    Random rng = new Random(new Crypto.SRand());
                    foreach (Player p in Players.ToList())
                    {
                        // Retrieve all plays from players who have played the
                        // required number of cards, and flatten the dictionary
                        // out into a list of identifier-card pairs.
                        List<Dictionary<int, WhiteCard>> cards = (from card in base.PlayedCards.ToList()
                                                                  where base.PickedList[card.Key]
                                                                  select card.Value).ToList();
                        for (
                            // This loop will iterate through the list of paired cards. In
                            // order to ensure each player receives cards in a random order,
                            // we select pairs at random by their index, and then remove them
                            // from the list so that they won't be revealed to the same player
                            // again.
                            int rnd = rng.Next(0, cards.Count - 1);
                            cards.Count > 0;
                            cards.Remove(cards[rnd]), rnd = rng.Next(0, cards.Count - 1)
                        )
                        {
                            // We create a list where we will store the information for each
                            // set of card choices.
                            List<string> cardParams = new List<string>();
                            // For each card in this specific set of choices, we iterate through
                            // and add the identifier and text to the list of strings.
                            foreach (var c in cards.ToList()[rnd])
                            {
                                cardParams.Add(c.Key.ToString());
                                cardParams.Add(c.Value.Text);
                            }
                            // We then send the bits of card information, turned into an
                            // array, to the player.
                            base.Parent.SendCommand(
                                CommandType.REVL,
                                cardParams.ToArray(),
                                p.ClientIdentifier
                            );
                        }
                    }
                }
                // If we don't have enough players to successfully play a gme, we'll
                // end the round by returning a null player to the parent Game class,
                // which will sent an RWIN without listed winner to all clients and
                // return to them their cards.
                else
                {
                    this._roundWinner = null;
                    // Invoke the event handler which will notify any listeners
                    // of the winner of the current round.
                    if (this.OnPlayerWin != null) this.OnPlayerWin.Invoke(_roundWinner);
                    // Exit the thread.
                    return;
                }

                // If we get to this point, we can reasonably assume that the game
                // is in a suitable state to continue, and so we won't perform any
                // checks.

                // The Czar is afforded twice the time given to normal players to
                // make their choice of the winner.
                var czpkTimer = new System.Timers.Timer(base.Parent.Parameters.TimeoutLimit * 2000);
                bool czpkTimerHasElapsed = false;
                czpkTimer.Elapsed += (s, e) =>
                {
                    // Stop the Card Czar, or any other players, from submitting CZPKs.
                    base.AllowCzpk = false;

                    // Inform all players that the Card Czar failed to pick within adequate time
                    // via a broadcast. The client doesn't need to be told the specific reason for
                    // a skipped round, but it might be nice if the players get told.
                    base.Parent.Broadcast(
                        String.Format(
                            "Card Czar {0} failed to pick winner within the time-limit and so the round has ended.",
                            base.Parent.CurrentRound.CardCzar.Nickname
                        )
                    );

                    // This round will be skipped, so we set the winner to null. Important to do
                    // this before we cause the while loop to end as assigning afterwards might cause
                    // a race condition where the variable is returned the appropriate value is set.
                    _roundWinner = null;

                    // Set the variable indicating whether the timer has elapsed to true,
                    // causing the blocking while loop to end and the flow of execution to continue.
                    czpkTimerHasElapsed = true;

                    // If the server is configured to kick players who time out, kick the Card Czar
                    // and inform them that they were kicked due to being inactive.
                    if (base.Parent.Parameters.KickOnTimeout)
                        base.Parent.Disconnect(base.Parent.CurrentRound.CardCzar.Nickname, "Disconnected due to inactivity.");
                };

                czpkTimer.Start();
                base.AllowCzpk = true;
                while (this._roundWinner == null ^ czpkTimerHasElapsed) ;
                base.AllowCzpk = false;
                czpkTimer.Stop();
                czpkTimer.Dispose();

                // Invoke the event handler which will notify any listeners
                // of the winner of the current round.
                if (this.OnPlayerWin != null) this.OnPlayerWin.Invoke(_roundWinner);
                // Exit the thread.
                return;
            });
        }

        public override void Stop()
        {
            // Return played cards to their players.
            foreach (var plays in base.PlayedCards)
            {
                foreach (var play in plays.Value)
                {
                    base.Parent.SendCommand(
                        CommandType.WHTE,
                        new string[2] { play.Key.ToString(), play.Value.Text },
                        plays.Key.ClientIdentifier
                    );
                }
            }

            // If the game-mode thread has not yet
            // been instantiated, there isn't anything
            // we can do to it, since there's nothing
            // to stop.
            if (this._gmThread == null)
                throw new InvalidOperationException("Cannot stop game-mode as it is not yet instantiated.");
            else this._gmThread.Abort();

            base.Stop();
        }

        public override event Game.PlayerEventHandler OnPlayerWin;

        public override void CommandCzpkHandler(long sender, string[] arguments)
        {
            if (arguments == null)
            {
                // If the client has attempted to submit a Czar Pick
                // without specifying any cards, we'll return an error
                // message to them regarding this, and then exit the function.
                base.Parent.SendCommand(
                    CommandType.UNRG,
                    "CZPK sent without arguments; invalid.",
                    sender
                );
                return;
            }

            // If the game mode has indicated that this is currently a period
            // in which CZPK commands are accepted and should be parsed.
            if (base.AllowCzpk)
            {
                // The player represented by the client identifier which sent this
                // command.
                Player pSender = base.Players.Single(p => p.ClientIdentifier == sender);
                
                // If the sender of the CZPK is the Card Czar for this round, we can
                // proceed as normal, since they're the one that's meant to be picking.
                if (sender == base.Parent.CurrentRound.CardCzar.ClientIdentifier)
                {
                    int cardId = 0;
                    // We must verify that the client sent a card identifier which
                    // can be parsed into an int. If they have not, an invalid
                    // card identifier has been sent.
                    if (int.TryParse(arguments[0], out cardId))
                    {
                        // We don't know, at this point, whether the card ID the
                        // Czar has sent us actually exists, so we have to wrap
                        // our check in a try-catch block to prevent crashes.
                        try
                        {
                            // If the provided card ID exists, we select the player who
                            // sent it in and set _roundWinner to indicate that they have
                            // won. If the card doesn't exist, an exception is thrown,
                            // and _roundWinner will remain null.
                            this._roundWinner = base.PlayedCards.Single(k => k.Value.ContainsKey(cardId)).Key;
                            // Since we've now selected a winner, we no longer need to
                            // execute this handler, and so we can set _allowCzpk to false,
                            // causing this function to exit much earlier if called again.
                            base.AllowCzpk = false;
                        }
                        catch (InvalidOperationException)
                        {
                            // If the card doesn't exist, this block will be executed and
                            // the Card Czar will be informed that the card they attempted
                            // to select does not exist or, rather, hasn't been played by
                            // any players.
                            base.Parent.SendCommand(
                                CommandType.UNRG,
                                String.Format(
                                    "Card has not been played ({0}).",
                                    cardId
                                ),
                                sender
                            );
                        }
                        // Exit out of the function.
                        return;
                    }
                    else
                    {
                        // If the card identifier is invalid, we inform
                        // the client of this, and provide the identifier
                        // which was invalid.
                        base.Parent.SendCommand(
                            CommandType.UNRG,
                            String.Format(
                                "Invalid card identifier sent ({0}).",
                                arguments[0]
                            ),
                            sender
                        );
                    }
                }
                else
                {
                    // If they aren't the Card Czar, however, we'll inform them of this via
                    // an UNRG and then exit the function, taking no further action on their
                    // command and continue waiting for the actual Card Czar to make a pick.
                    base.Parent.SendCommand(
                        CommandType.CZPK,
                        "You are not the Card Czar.",
                        sender
                    );
                    return;
                }
            }
            else
            {
                // If CZPKs are not currently being accepted, we'll
                // respond with an UNRG and exit out of the function.
                // Since card picks aren't persistent across rounds in
                // the same way drawn cards are, we won't do anything
                // to "return" the card to the client.
                base.Parent.SendCommand(
                    CommandType.UNRG,
                    "CZPK commands are not being accepted at this time.",
                    sender
                );
                return;
            }
        }
    }
}

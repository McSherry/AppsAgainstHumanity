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
        /// True when the server is expecting to receive
        /// a CZPK command. When false, the command handler
        /// for CZPKs will not function.
        /// </summary>
        private bool _allowCzpk = false;
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

        public override Player Start()
        {
            // Create the thread on which this round's code
            // will execute. We must create a new thread so
            // as not to block the main threads when waiting,
            // and so we can more easily terminate the round
            // in the event of a disconnection.
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

                /* REWRITE THIS SHIT
                 * Stop coding when tired. This crap should stay in Round, but you probably
                 * need to modify your pick handler for this class to set the value of a given
                 * base.PickedList[Player] when the expected number of cards are played by
                 * said player (if this hasn't already been done).
                 * 
                 * These classes really are a mess. You should have planned this out more
                 * thoroughly before you just wrote the implementation of the Standard game mode
                 * and accidentally made it far too integrated with the Round class.
                 */
            });

            throw new NotImplementedException();
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
            if (_allowCzpk)
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
                            this._allowCzpk = false;
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

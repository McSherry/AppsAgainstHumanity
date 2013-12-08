using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public delegate void CommandHandler(long sender, string[] arguments);

	public class AAHProtocolWrapper
	{
		/// <summary>
		/// Handles sending and receiving data
		/// </summary>
		public ITransmittable Transmitter { get; private set; }
		/// <summary>
		/// ASCII NUL char
		/// </summary>
		private const char NUL = (char)0;
		/// <summary>
		/// Maps commands to their handlers
		/// </summary>
		private Dictionary<CommandType, CommandHandler> Commands = new Dictionary<CommandType, CommandHandler>();
		/// <summary>
		/// A chronological queue of commands that did not have handlers present at the time of firing.
		/// </summary>
		private Queue<Command> QueuedCommands = new Queue<Command>();
		/// <summary>
		/// Used for building the command strings
		/// </summary>
		private StringBuilder CommandBuilder = new StringBuilder();
		/// <summary>
		/// Checks whether the unhandled command queue contains any commands, and returns the result.
		/// </summary>
		public bool HasQueuedCommand
		{
			get { return QueuedCommands.Count > 0; }
		}
		/// <summary>
		/// Create a protocol wrapper, to be wrapped around a client or server
		/// </summary>
		/// <param name="inputOutput"></param>
		public AAHProtocolWrapper(ITransmittable transmitter)
		{
			Transmitter = transmitter;
			transmitter.OnDataAvailable += HandleDataAvailable;
		}
		/// <summary>
		/// Registers a command handler, so it will be called when the belonging command is received.
		/// </summary>
		/// <param name="type">The CommandType this handler should react to.</param>
		/// <param name="handler">A delegate that will be called when the command is received.</param>
		public void RegisterCommandHandler(CommandType type, CommandHandler handler)
		{
			Commands.Add(type, handler);
		}
		/// <summary>
		/// Unregisters a command handler, so it will no longer be called when the belonging command is received.
		/// </summary>
		/// <param name="type">The type of command that the wrapper should no longer react to.</param>
		public void UnregisterCommandHandler(CommandType type)
		{
			Commands.Remove(type);
		}
		/// <summary>
		/// Dequeues a command from the unhandled command queue.
		/// </summary>
		/// <returns>The command that was dequeued.</returns>
		public Command GetNextQueuedCommand()
		{
			return QueuedCommands.Dequeue();
		}
		/// <summary>
		/// Unregisters all command handlers.
		/// </summary>
		public void UnregisterAllHandlers()
		{
			Commands.Clear();
		}
		/// <summary>
		/// Process data made available by the networking library, to turn it into commands.
		/// </summary>
		/// <param name="data">The line of data that was sent, without the delimiter.</param>
		/// <param name="clientId">The id of the client who sent the line. Is simply passed along to the upper layers.</param>
		private void HandleDataAvailable(string data, long clientId)
		{
			Console.WriteLine("[RECV]\t[{0}]\t{1}", clientId, data);

			// Command is always the first four bytes, so extract those
			var command = data.Substring(0, 4);
			string[] args = null;
			// Check if the command has any arguments
			if (data.Length > 4) {
				var arglist = data.Substring(4);
				// Create an array of arguments by splitting the argument on NUL chars, according to the protocol.
				args = arglist.Split(new char[] { NUL });
			}
			if (command == "PING") {
				SendCommand(CommandType.PONG, 0);
			}

			// Attempt to parse the command into a CommandType. If it succeeds, verify that a handler has been
            // registered for said command. If one has been registered, call it. If one has not, don't.
            CommandType ct;
            if (Enum.TryParse(command, out ct))
            {
				// If a handler for the command has been set up, call it.
				if (Commands.ContainsKey(ct)) Commands[ct](clientId, args);
				// If no handler has been set up, put the command in the queue so it can optionally be handled later.
				else QueuedCommands.Enqueue(new Command { Arguments = args, Sender = clientId, Type = ct });
            }
            else
            {
                // If parsing fails, verify that there is a handler bound to the _InternalInvalid type,
                // and call said handler if it is present.
                if (Commands.ContainsKey(CommandType._InternalInvalid)) 
                    Commands[CommandType._InternalInvalid](clientId, args);
            }
		}
		/// <summary>
		/// Sends a command with a single argument to the specified client. If the wrapped ITransmittable is a client, 
		/// the clientId variable is discarded.
		/// </summary>
		/// <param name="cmd">The command to send</param>
		/// <param name="argument">The argument belonging to the command. Use the SendData(CommandType, string[], long) overload
		/// when multiple arguments need to be passed.</param>
		/// <param name="clientId">The ID of the client that should receive the command. This variable is discarded if the wrapped
		/// ITransmittable is of type NetLibClient, and in that case, may be left at its default value.</param>
		public bool SendCommand(CommandType cmd, string argument, long clientId = 0)
		{
			return SendCommand(cmd, argument == null ? (string[])null : new string[] { argument }, clientId);
		}

		public bool SendCommand(CommandType cmd, long clientId)
		{
			return SendCommand(cmd, new string[0], clientId);
		}

		/// <summary>
		/// Sends a command to the specified client. If the wrapped ITransmittable is a client,  the clientId variable is discarded.
		/// </summary>
		/// <param name="cmd">The command to send</param>
		/// <param name="args">The arguments belonging to the command. Use the SendData(CommandType, string, long) overload
		/// when only one argument needs to be passed. Keep at its default value of null when no arguments should be passed.</param>
		/// <param name="clientId">The ID of the client that should receive the command. This variable is discarded if the wrapped
		/// ITransmittable is of type NetLibClient, and in that case, may be left at its default value.</param>
		public bool SendCommand(CommandType cmd, string[] args, long clientId = 0)
		{
			if (args == null) {
				args = new string[0];
			}
			// Remove the previous command still left in the command builder
			CommandBuilder.Clear();
			// First append the command
			CommandBuilder.Append(cmd.ToString());
			// Now append the parameters, separated by ASCII NULs
			CommandBuilder.Append(string.Join(NUL.ToString(), args));
			string data = CommandBuilder.ToString();
			Console.WriteLine("[SEND]\t[{0}]\t{1}", clientId, data);
			return Transmitter.Send(data, clientId);
		}
	}
}

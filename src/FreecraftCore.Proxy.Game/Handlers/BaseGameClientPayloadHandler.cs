﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace FreecraftCore
{
	/// <summary>
	/// Simplied type alias for game handlers that handle Client payloads sent from the client.
	/// </summary>
	[ClientPayloadHandler]
	public abstract class BaseGameClientPayloadHandler<TSpecificPayloadType> : IPeerPayloadSpecificMessageHandler<TSpecificPayloadType, GamePacketPayload, IProxiedMessageContext<GamePacketPayload, GamePacketPayload>>
		where TSpecificPayloadType : GamePacketPayload
	{
		/// <summary>
		/// The logger for the handler.
		/// </summary>
		protected ILog Logger { get; }

		/// <inheritdoc />
		protected BaseGameClientPayloadHandler([NotNull] ILog logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public abstract Task OnHandleMessage(IProxiedMessageContext<GamePacketPayload, GamePacketPayload> context, TSpecificPayloadType payload);

		/// <inheritdoc />
		public async Task HandleMessage(IProxiedMessageContext<GamePacketPayload, GamePacketPayload> context, TSpecificPayloadType payload)
		{
			if(Logger.IsInfoEnabled)
				Logger.Info($"Client Sent: {payload.GetOperationCode()}:{((int)payload.GetOperationCode()):X}");

			try
			{
				await OnHandleMessage(context, payload);
			}
			catch(Exception e)
			{
				if(Logger.IsErrorEnabled)
					Logger.Error($"Encountered Error in Handler: {GetType().Name} Exception: {e.Message} \n\n Stack: {e.StackTrace}");
			}
		}
	}
}

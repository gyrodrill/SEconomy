﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfje.Plugins.SEconomy.Extensions;

namespace Wolfje.Plugins.SEconomy.CmdAliasModule.Extensions {
	public static class TSPlayerExtensions {

		/// <summary>
		/// This is a copy of TShocks handlecommand method, sans the permission checks
		/// </summary>
		public static bool PermissionlessInvoke(this TShockAPI.TSPlayer player, string text)
		{
			IEnumerable<TShockAPI.Command> cmds;
			List<string> args;
			string cmdName;
			string cmdText;

			if (string.IsNullOrEmpty(text)) {
				return false;
			}

			cmdText = text.Remove(0, 1);
			args = typeof(TShockAPI.Commands).CallPrivateMethod<List<string>>(true, "ParseParameters", cmdText);

			if (args.Count < 1) {
				return false;
			}

			cmdName = args[0].ToLower();
			args.RemoveAt(0);
			cmds = TShockAPI.Commands.ChatCommands.Where(c => c.HasAlias(cmdName));

			if (Enumerable.Count(cmds) == 0) {
				if (player.AwaitingResponse.ContainsKey(cmdName)) {
					Action<TShockAPI.CommandArgs> call = player.AwaitingResponse[cmdName];
					player.AwaitingResponse.Remove(cmdName);
					call(new TShockAPI.CommandArgs(cmdText, player, args));
					return true;
				}
				player.SendErrorMessage("命令错误。输入/help 查看可用命令列表。");
				return true;
			}

			foreach (TShockAPI.Command cmd in cmds) {
				if (!cmd.AllowServer && !player.RealPlayer) {
					player.SendErrorMessage("你只能在游戏中使用这个命令。");
				} else {
					if (cmd.DoLog)
						TShockAPI.TShock.Utils.SendLogs(string.Format("{0} 执行了: /{1}.", player.Name, cmdText), Microsoft.Xna.Framework.Color.Red);
					cmd.RunWithoutPermissions(cmdText, player, args);
				}
			}

			return true;
		}

	}
}

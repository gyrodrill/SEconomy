using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using TShockAPI;

namespace Wolfje.Plugins.SEconomy.CmdAliasModule {
	public class Configuration {

		public List<AliasCommand> CommandAliases = new List<AliasCommand>();

		/// <summary>
		/// Loads a configuration file and deserializes it from JSON
		/// </summary>
		public static Configuration LoadConfigurationFromFile(string Path)
		{
			Configuration config = null;
			string confDirectory = System.IO.Path.Combine(Path, "aliascmd.conf.d");

			try {
				string fileText = System.IO.File.ReadAllText(Path);
				config = JsonConvert.DeserializeObject<Configuration>(fileText);
			} catch (Exception ex) {
				if (ex is System.IO.FileNotFoundException || ex is System.IO.DirectoryNotFoundException) {
					TShock.Log.ConsoleError("CmdAlias配置:找不到文件或目录。正在创建。");

					config = Configuration.NewSampleConfiguration();
					config.SaveConfiguration(Path);
				} else if (ex is System.Security.SecurityException) {
					TShock.Log.ConsoleError("CmdAlias配置:没有权限读取文件" + Path);
				} else {
					TShock.Log.ConsoleError("CmdAlias配置:发生错误" + ex.ToString());
				}
			}

			if (Directory.Exists(confDirectory) == false) {
				try {
					Directory.CreateDirectory(confDirectory);
				} catch {
				}
			}	

			if (Directory.Exists(confDirectory) == false) {
				return config;
			}

			/*
			 * Command aliases in .json files in the subdirectory "aliascmd.conf.d"
			 * will be loaded into the main configuration.
			 */
			foreach (string aliasFile in Directory.EnumerateFiles(confDirectory, "*.json")) {
				string fileText = null;
				Configuration extraConfig = null;

				if (string.IsNullOrEmpty(aliasFile)
				    || File.Exists(aliasFile) == false) {
					continue;
				}

				try {
					fileText = File.ReadAllText(aliasFile);
				} catch {
				}

				if (string.IsNullOrEmpty(fileText) == true) {
					continue;
				}

				try {
					extraConfig = JsonConvert.DeserializeObject<Configuration>(fileText);
				} catch {
				}
			
				if (extraConfig == null) {
					continue;
				}

				foreach (AliasCommand alias in extraConfig.CommandAliases) {
					if (config.CommandAliases.FirstOrDefault(i => i.CommandAlias == alias.CommandAlias) != null) {
						TShock.Log.ConsoleError("AliasCmd警告: 文件{1}中出现重复命令{0}，已忽略。",
							alias.CommandAlias, System.IO.Path.GetFileName(aliasFile));
						continue;
					}

					config.CommandAliases.Add(alias);
				}
			}

			return config;
		}

		public static Configuration NewSampleConfiguration()
		{
			Configuration newConfig = new Configuration();

			newConfig.CommandAliases.Add(AliasCommand.Create("testparms", "", "0c", "", 0, "/bc 参数1 2 3: $1 $2 $3", "/bc 参数1-3: $1-3", "/bc 参数2到该行结尾: $2-"));
			newConfig.CommandAliases.Add(AliasCommand.Create("testrandom", "", "0c", "", 0, "/bc 随机数: $random(1,100)"));
			newConfig.CommandAliases.Add(AliasCommand.Create("impersonate", "", "0c", "", 0, "$runas($1,/me 原文已屏蔽，这里只留个随机数 $random(1,100) 。)"));

			return newConfig;
		}

		public void SaveConfiguration(string Path)
		{

			try {
				string config = JsonConvert.SerializeObject(this, Formatting.Indented);

				System.IO.File.WriteAllText(Path, config);

			} catch (Exception ex) {

				if (ex is System.IO.DirectoryNotFoundException) {
					TShock.Log.ConsoleError("CmdAlias配置:没有找到文件夹" + Path);

				} else if (ex is UnauthorizedAccessException || ex is System.Security.SecurityException) {
					TShock.Log.ConsoleError("CmdAlias配置:没有权限" + Path);
				} else {
					TShock.Log.ConsoleError("CmdAlias配置:发生错误" + Path);
					throw;
				}
			}

		}

	}

 
}

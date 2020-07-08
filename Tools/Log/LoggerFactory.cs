﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Tools.Log
{
	public class LoggerFactory
	{
		private readonly XmlDocument document = new XmlDocument();
		private static readonly object o=new object();//公共对象锁
		private string configPath = string.Empty;//配置文件；路径
		private string path;//日志路径
		private string className;
		private string assembly;
		private bool flag = true;
		private XmlNode target;
		private static Dictionary<string, GeneralLogger> logDict = new Dictionary<string, GeneralLogger>();

		private const string DefaultClass = "Tools.Log.CommonLogger";
		private const string DefaultAssembly = "Tools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
		/// <summary>
		///  建造日志类
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		private GeneralLogger Bulid(string code)
		{
			//模板方法
			LoadConfig();
			GetProperties(code);
			GeneralLogger logger = GetLogger();
			SetProperties(ref logger);
			SetDict(code,ref logger);
			return logDict[code];
		}

		/// <summary>
		///  获取日志实例
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public GeneralLogger GetInstance(string moduleCode)
		{
			return GetInstance(moduleCode, string.Empty);
		}
		public GeneralLogger GetInstance(string moduleCode, string configPath)
		{
			this.configPath = configPath;
			if (logDict.ContainsKey(moduleCode))
			{
				return logDict[moduleCode];
			}
			else
			{
				return Bulid(moduleCode);
			}
		}
		private void LoadConfig()
		{
			if (!string.IsNullOrWhiteSpace(configPath))
			{
				document.Load(configPath); 
			}
		}
		private void GetProperties(string code)
		{
			if (!string.IsNullOrWhiteSpace(configPath))
			{
				target = document.SelectSingleNode("//Log[@Code='" + code + "']");
				path = target.Attributes["FullPath"].Value;
				className = target.Attributes["Class"].Value;
				assembly = target.Attributes["Assembly"].Value;
				flag = System.Convert.ToBoolean(target.Attributes["Print"].Value); 
			}
			else
			{
				path = "c:\\Log\\"+code+"\\";
				className = DefaultClass;
				assembly = DefaultAssembly;
			}
		}
		
		private GeneralLogger GetLogger()
		{
			Type t = Assembly.Load(assembly).GetType(className);
			GeneralLogger logger = Activator.CreateInstance(t) as GeneralLogger;
			return logger;
		}

		private void SetProperties(ref GeneralLogger logger)
		{
			logger.FullFilePath = path;
			logger.Open = flag;
		}
		private void SetDict(string code,ref GeneralLogger logger)
		{
			if (!logDict.ContainsKey(code))
			{
				lock (o)
				{
					if (!logDict.ContainsKey(code))
					{
						logDict.Add(code, logger); 
					}
				} 
			}
		}

	}
}

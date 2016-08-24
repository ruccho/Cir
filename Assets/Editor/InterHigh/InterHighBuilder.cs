using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace UnityInterHigh
{
	public class Builder
	{

		public static void BuildWindows()
		{
			Build(new WindowsPlayer());
		}
		public static void BuildMac()
		{
			Build(new MacPlayer());
		}
		public static void BuildWebGL()
		{
			Build(new WebGLPlayer());
		}

		public static void BuildProject()
		{
			var title = "Unityインターハイ - ビルドエクスポーター";
			var info = "プロジェクト エクスポート中...";

			EditorUtility.DisplayProgressBar(title, info, 0);
			using (var memoryStreamOut = new MemoryStream())
			{
				using (ZipOutputStream zipOutStream = new ZipOutputStream(memoryStreamOut))
				{
					CompressFolder("Assets", zipOutStream, "");
					EditorUtility.DisplayProgressBar(title, info, 0.5f);
					CompressFolder("ProjectSettings", zipOutStream, "");
					EditorUtility.DisplayProgressBar(title, info, 0.75f);
					CompressFolder("Library", zipOutStream, "");
					zipOutStream.Finish();
					zipOutStream.Close();
				}

				File.WriteAllBytes(string.Format("{0}.zip", Path.Combine("build", "project")), memoryStreamOut.ToArray());
			}
			EditorUtility.ClearProgressBar();
		}

		static void Build(Player player)
		{
			EditorUtility.DisplayProgressBar("Unityインターハイ - ビルドエクスポーター", string.Format("{0} エクスポート中...", player.GetType().Name), 0);
			if (player != null)
			{
				player.Build();
				EditorUtility.DisplayProgressBar("Unityインターハイ - ビルドエクスポーター", string.Format("{0} エクスポート中...", player.GetType().Name), 0.8f);
				player.Archive();
			}
			EditorUtility.ClearProgressBar();
		}

		internal static void CompressFolder(string path, ZipOutputStream zipStream, string rootPath)
		{
			string[] files = Directory.GetFiles(path);

			foreach (string filename in files)
			{
				var fi = new FileInfo(filename);

				var entryName = ZipEntry.CleanName(filename.Substring(rootPath.Length));
				var newEntry = new ZipEntry(entryName);
				newEntry.DateTime = fi.LastWriteTime;
				newEntry.Size = fi.Length;

				zipStream.PutNextEntry(newEntry);

				byte[] buffer = new byte[4096];
				using (FileStream streamReader = File.OpenRead(filename))
				{
					StreamUtils.Copy(streamReader, zipStream, buffer);
				}
				zipStream.CloseEntry();
			}
			string[] folders = Directory.GetDirectories(path);
			foreach (string folder in folders)
			{
				CompressFolder(folder, zipStream, rootPath);
			}
		}
	}

	public abstract class Player
	{
		internal string location = "build";

		internal abstract string filename { get; }

		internal abstract string locationPath { get; }

		public abstract Player Build();

		public virtual void Archive()
		{
			using (var memoryStreamOut = new MemoryStream())
			{
				using (ZipOutputStream zipOutStream = new ZipOutputStream(memoryStreamOut))
				{
					Builder.CompressFolder(locationPath, zipOutStream, location);
					zipOutStream.Finish();
					zipOutStream.Close();
				}
				File.WriteAllBytes(string.Format("{0}.zip", locationPath), memoryStreamOut.ToArray());
				Directory.Delete(locationPath, true);
			}
		}

		internal string[] GetEnabledScenes()
		{
			return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
		}
	}

	public class WebGLPlayer : Player
	{
		public override Player Build()
		{
			BuildPipeline.BuildPlayer(GetEnabledScenes(),
				locationPath,
				BuildTarget.WebGL,
				BuildOptions.None);
			return this;
		}

		internal override string filename
		{
			get { return "webgl"; }
		}

		internal override string locationPath
		{
			get
			{
				return string.Format("{0}/{1}", location, filename);
			}
		}
	}

	public class MacPlayer : Player
	{
		internal override string filename
		{
			get { return "mac"; }
		}

		internal override string locationPath
		{
			get
			{
				return string.Format("{0}/{1}.app", location, filename);
			}
		}

		public override Player Build()
		{
			BuildPipeline.BuildPlayer(GetEnabledScenes(),
				locationPath,
				BuildTarget.StandaloneOSXUniversal,
				BuildOptions.None);
			return this;
		}
	}

	public class WindowsPlayer : Player
	{
		internal override string filename
		{
			get { return "windows"; }
		}

		internal override string locationPath
		{
			get
			{
				return string.Format("{0}/windows/{1}.exe", location, filename);
			}
		}

		public override Player Build()
		{
			Directory.CreateDirectory(string.Format("{0}/windows", location));
			BuildPipeline.BuildPlayer(GetEnabledScenes(),
				locationPath,
				BuildTarget.StandaloneWindows64,
				BuildOptions.None);
			return this;
		}

		public override void Archive()
		{
			var rootPath = Path.Combine(location, filename);
			foreach (var file in Directory.GetFiles(rootPath, "*.pdb"))
			{
				File.Delete(file);
			}
			using (var memoryStreamOut = new MemoryStream())
			{
				using (ZipOutputStream zipOutStream = new ZipOutputStream(memoryStreamOut))
				{
					Builder.CompressFolder(rootPath, zipOutStream, rootPath);
					zipOutStream.Finish();
					zipOutStream.Close();
				}
				File.WriteAllBytes(string.Format("{0}/{1}.zip", location, filename), memoryStreamOut.ToArray());
				Directory.Delete(rootPath, true);
			}
		}
	}
}
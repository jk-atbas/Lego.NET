namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Settings;

internal static class JsonManifests
{
	public const string Empty = """
	{
		"platforms": {}
	}
	""";

	public const string EmptyWithOses = """
	{
	    "platforms": {
	        "windows": [],
	        "linux": []
	    }
	}                     
	""";

	public const string X64Windows = """
	 {
	    "platforms": {
	        "windows": [
	            {
	                "arch": "x64",
	                "version": "4.31.0",
	                "path": "native/win/x64/lego.exe"
	            }
	        ],
	        "linux": []
	    }
	 }
	 """;

	public const string X64WindowsLinux = """
	{
		"platforms": {
		    "windows": [
		        {
		            "arch": "x64",
		            "version": "4.31.0",
		            "path": "native/win/x64/lego.exe"
		        }
		    ],
		    "linux": [
				{
					"arch": "x64",
					"version": "4.31.0",
					"path": "native/linux/x64/lego"
				}
		    ]
		}
	}
	""";
}

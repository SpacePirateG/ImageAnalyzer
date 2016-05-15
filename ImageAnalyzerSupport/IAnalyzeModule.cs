using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAnalyzer
{
	public interface IAnalyzeModule
	{
		String ModuleName {
			get;
		}

		Task<ImageInfo> Analyze(byte[] image);
	}
}

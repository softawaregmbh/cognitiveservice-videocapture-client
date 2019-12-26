using softaware.ViewPort.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.UI
{
    public class AnalyzerStatisticsViewModel : NotifyPropertyChanged
	{
		private string name;
		private int count;
		private TimeSpan lastDuration;

		public double CostsPerRequest { get; set; }

		public string Name
		{
			get { return name; }
			set { this.SetProperty(ref this.name, value); }
		}
		
		public int Count
		{
			get { return count; }
			set 
			{ 
				this.SetProperty(ref this.count, value);
				this.RaisePropertyChanged(nameof(this.Costs));
			}
		}
		
		public double Costs
		{
			get { return this.Count * this.CostsPerRequest; }
		}

		public TimeSpan LastDuration
		{
			get { return lastDuration; }
			set { this.SetProperty(ref this.lastDuration, value); }
		}

	}
}

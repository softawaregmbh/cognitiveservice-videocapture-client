// <copyright file="AnalyzerStatisticsViewModel.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using softaware.ViewPort.Core;

namespace VideoCapture.UI
{
    /// <summary>
    /// View model for analyzer statistics.
    /// </summary>
    /// <seealso cref="softaware.ViewPort.Core.NotifyPropertyChanged" />
    public class AnalyzerStatisticsViewModel : NotifyPropertyChanged
    {
        private string name;
        private int count;
        private TimeSpan lastDuration;

        /// <summary>
        /// Gets or sets the costs per request.
        /// </summary>
        /// <value>
        /// The costs per request.
        /// </value>
        public double CostsPerRequest { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        /// <summary>
        /// Gets or sets the request count.
        /// </summary>
        /// <value>
        /// The request count.
        /// </value>
        public int Count
        {
            get
            {
                return this.count;
            }

            set
            {
                this.SetProperty(ref this.count, value);
                this.RaisePropertyChanged(nameof(this.Costs));
            }
        }

        /// <summary>
        /// Gets the total costs for this session.
        /// </summary>
        /// <value>
        /// The total costs.
        /// </value>
        public double Costs
        {
            get { return this.Count * this.CostsPerRequest; }
        }

        /// <summary>
        /// Gets or sets the duration of the last request.
        /// </summary>
        /// <value>
        /// The duration of the last request.
        /// </value>
        public TimeSpan LastDuration
        {
            get { return this.lastDuration; }
            set { this.SetProperty(ref this.lastDuration, value); }
        }
    }
}

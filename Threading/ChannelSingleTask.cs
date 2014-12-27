// -----------------------------------------------------------------------
// <copyright file="ChannelSingleTask.cs" company="-">
// Copyright (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
// -----------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Laroux.ScabbiaLibrary.Threading
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Tasslehoff.Library.Helpers;

    /// <summary>
    /// ChannelSingleTask class.
    /// </summary>
    public class ChannelSingleTask : IDisposable
    {
        // fields

        /// <summary>
        /// The status sync
        /// </summary>
        private readonly object statusSync;

        /// <summary>
        /// The action
        /// </summary>
        private Action<object> action;

        /// <summary>
        /// The parameter
        /// </summary>
        private object parameter;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// The status
        /// </summary>
        private ChannelTaskStatus status;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelSingleTask"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="action">The action.</param>
        public ChannelSingleTask(object parameter, Action<object> action) : this()
        {
            this.action = action;
            this.parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelSingleTask"/> class.
        /// </summary>
        /// <param name="action">The action</param>
        public ChannelSingleTask(Action<object> action) : this()
        {
            this.action = action;
            this.parameter = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelSingleTask"/> class.
        /// </summary>
        protected ChannelSingleTask()
        {
            this.cancellationTokenSource = new CancellationTokenSource();

            this.statusSync = new object();
            this.status = ChannelTaskStatus.NotStarted;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ChannelSingleTask"/> class.
        /// </summary>
        ~ChannelSingleTask()
        {
            this.Dispose(false);
        }

        // events

        /// <summary>
        /// Occurs when [status changed].
        /// </summary>
        public event EventHandler<ChannelTaskStatusChangedEventArgs> StatusChanged;

        // properties

        /// <summary>
        /// Gets the status.
        /// </summary>
        public ChannelTaskStatus Status
        {
            get
            {
                lock (this.statusSync)
                {
                    return this.status;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ChannelTaskQueue"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }
            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Does the task.
        /// </summary>
        /// <returns>The task object</returns>
        public Task DoTask()
        {
            return Task.Factory.StartNew(this.TaskAction, this.parameter, this.cancellationTokenSource.Token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Tasks the action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected void TaskAction(object parameter)
        {
            if (this.cancellationTokenSource.IsCancellationRequested)
            {
                lock (this.statusSync)
                {
                    this.status = ChannelTaskStatus.Cancelled;
                    this.OnStatusChanged();
                }

                return;
            }

            lock (this.statusSync)
            {
                this.status = ChannelTaskStatus.Running;
                this.OnStatusChanged();
            }

            this.action(parameter);

            lock (this.statusSync)
            {
                this.status = ChannelTaskStatus.Finished;
                this.OnStatusChanged();
            }
        }

        /// <summary>
        /// Called when [status changed].
        /// </summary>
        protected void OnStatusChanged()
        {
            if (this.StatusChanged != null)
            {
                this.StatusChanged(this, new ChannelTaskStatusChangedEventArgs(this.status, this.parameter));
            }
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            VariableHelpers.CheckAndDispose<CancellationTokenSource>(ref this.cancellationTokenSource);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "cancellationTokenSource")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }
    }
}
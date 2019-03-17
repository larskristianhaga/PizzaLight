﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Quartz.Xml.JobSchedulingData20;

namespace PizzaLight.Tests.Unit
{
    [TestFixture]
    public class SchedulerTests
    {
        private int i= 0;
        private CancellationTokenSource _cts;
        private Timer _timer;

        [Test]
        public async Task RepeatingTaskUsingTimerCanBeCancelled()
        {
            _cts = new CancellationTokenSource();
            _timer = new Timer(async state =>await DoSomething(state), null, TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(50));
            await Task.Delay(1000, _cts.Token)
                .ContinueWith(t => Assert.That(t.IsCanceled));
            Assert.That(i>= 3);
        }

        private async Task DoSomething(object state)
        {
            Console.WriteLine("Repetition " + i);
            if(i >= 3)
            {
                _cts.Cancel(false);
                _timer.Dispose();
            }
            i++;
        }
    }
}
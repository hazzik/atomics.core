﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace System.Threading.Atomics.Tests
{
    public class AtomicReferenceConstructorTests
    {
        [Fact]
        public void AtomicReference_MemoryOrder_Should_Fail()
        {
#pragma warning disable 612, 618
            Assert.Throws<ArgumentException>(() => new AtomicReference<object>(MemoryOrder.Consume));
#pragma warning restore 612, 618
        }

        [Fact]
        public void AtomicReference_MemoryOrder_Should_Success()
        {
            GC.KeepAlive(new AtomicReference<object>(MemoryOrder.Acquire));
            GC.KeepAlive(new AtomicReference<object>(MemoryOrder.Release));
            GC.KeepAlive(new AtomicReference<object>(MemoryOrder.AcqRel));
            GC.KeepAlive(new AtomicReference<object>(MemoryOrder.SeqCst));
        }

        [Fact]
        public void AtomicReference_MemoryOrder_Default_Should_Success()
        {
            GC.KeepAlive(new AtomicReference<object>());
        }

        [Fact]
        public void AtomicReference_InitialValue_With_MemoryOrder_Should_Fail()
        {
#pragma warning disable 612, 618
            Assert.Throws<ArgumentException>(() => new AtomicReference<object>(true, MemoryOrder.Consume));
#pragma warning restore 612, 618
        }

        [Fact]
        public void AtomicReference_InitialValue_With_MemoryOrder_Should_Success()
        {
            GC.KeepAlive(new AtomicReference<object>(true, MemoryOrder.Acquire));
            GC.KeepAlive(new AtomicReference<object>(true, MemoryOrder.Release));
            GC.KeepAlive(new AtomicReference<object>(true, MemoryOrder.AcqRel));
            GC.KeepAlive(new AtomicReference<object>(true, MemoryOrder.SeqCst));
        }

        [Fact]
        public void AtomicReference_InitialValue_With_MemoryOrder_Default_Should_Success()
        {
            GC.KeepAlive(new AtomicReference<object>(true));
        }
    }
}

﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using ShaderTools.CodeAnalysis.QuickInfo;
using ShaderTools.CodeAnalysis.Text.Shared.Extensions;

namespace ShaderTools.CodeAnalysis.Editor.Implementation.IntelliSense.QuickInfo
{
    internal class Model
    {
        public ITextVersion TextVersion { get; }
        public QuickInfoItem Item { get; }
        public IQuickInfoProvider Provider { get; }
        public bool TrackMouse { get; }

        public Model(
            ITextVersion textVersion,
            QuickInfoItem item,
            IQuickInfoProvider provider,
            bool trackMouse)
        {
            this.TextVersion = textVersion;
            this.Item = item;
            this.Provider = provider;
            this.TrackMouse = trackMouse;
        }

        internal SnapshotSpan GetCurrentSpanInSnapshot(TextSpan originalSpan, ITextSnapshot textSnapshot)
        {
            var trackingSpan = this.TextVersion.CreateTrackingSpan(originalSpan.ToSpan(), SpanTrackingMode.EdgeInclusive);
            return trackingSpan.GetSpan(textSnapshot);
        }
    }
}

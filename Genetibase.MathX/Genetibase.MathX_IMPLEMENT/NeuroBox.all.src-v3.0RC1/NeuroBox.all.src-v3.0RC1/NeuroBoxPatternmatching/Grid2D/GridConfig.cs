#region Copyright 2001-2006 Christoph Daniel Rüegg [GNU Public License]
/*
NeuroBox, a library for neural network generation, propagation and training
Copyright (c) 2001-2006, Christoph Daniel Rueegg, http://cdrnet.net/. All rights reserved.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

using NeuroBox;
using NeuroBox.PatternMatching;

namespace NeuroBox.PatternMatching.Grid2D
{
    public class Grid2DConfig
    {
        private ConfigNode _config;

        public Grid2DConfig(ConfigNode config)
        {
            _config = config;

            _all2allEn = new ConfigItem<bool>(_config, "All2AllEnable", true);
            _verticalLinesEn = new ConfigItem<bool>(_config, "VerticalLinesEnable", true);
            _horizontalLinesEn = new ConfigItem<bool>(_config, "HorizontalLinesEnable", true);
            _ringsEn = new ConfigItem<bool>(_config, "RingsEnable", true);
            _littleSquaresEn = new ConfigItem<bool>(_config, "LittleSquaresEnable", true);
        }

        public ConfigNode Node
        {
            get { return _config; }
            set { _config = value; }
        }

        private ConfigItem<bool> _all2allEn;
        /// <summary>Whether to connect all neurons together.</summary>
        public ConfigItem<bool> All2AllEnable { get { return _all2allEn; } }

        private ConfigItem<bool> _verticalLinesEn;
        /// <summary>Whether to connect neurons in vertical lines.</summary>
        public ConfigItem<bool> VerticalLinesEnable { get { return _verticalLinesEn; } }
        private ConfigItem<bool> _horizontalLinesEn;
        /// <summary>Whether to connect neurons in horizontal lines.</summary>
        public ConfigItem<bool> HorizontalLinesEnable { get { return _horizontalLinesEn; } }

        private ConfigItem<bool> _ringsEn;
        /// <summary>Whether to connect neurons in rings.</summary>
        public ConfigItem<bool> RingsEnable { get { return _ringsEn; } }

        private ConfigItem<bool> _littleSquaresEn;
        /// <summary>Whether to connect neurons in little squares.</summary>
        public ConfigItem<bool> LittleSquaresEnable { get { return _littleSquaresEn; } }

    }
}

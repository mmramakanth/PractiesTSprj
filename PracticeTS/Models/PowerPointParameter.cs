using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace PracticeTS.Models
{
    public class PowerPointParameter
    {

        #region Name
        /// <summary>
        /// Gets or sets the Name of this PowerPointParameter.
        /// </summary>
        public string Name { get; set; }
        #endregion


        #region Text
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public string Text { get; set; }
        #endregion


        #region Image
        /// <summary>
        /// Gets or sets the Image of this PowerPointParameter.
        /// </summary>
        public FileInfo Image { get; set; }
        #endregion

        #region Color
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public Color color { get; set; }
        #endregion

        #region TextColor
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public string textcolor { get; set; }
        #endregion

        #region FontSize
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public int FontSize { get; set; }
        #endregion

        #region Bold
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public bool bold { get; set; }
        #endregion

        #region Italic
        /// <summary>
        /// Gets or sets the Text of this PowerPointParameter.
        /// </summary>
        public bool italic { get; set; }
        #endregion
    }
}

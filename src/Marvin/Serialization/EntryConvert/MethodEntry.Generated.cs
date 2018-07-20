/*
 * This code was generated by the CGbR generator on 14.11.2017. Any manual changes will be lost on the next build.
 * 
 * For questions or bug reports please refer to https://github.com/Toxantron/CGbR or contact the distributor of the
 * 3rd party generator target.
 */

//TODO: Regenerate files using CGbR if fix of #30 is available in Release build: https://github.com/Toxantron/CGbR/issues/30

namespace Marvin.Serialization
{
    /// <summary>
    /// Auto generated class by CGbR project
    /// </summary>
    public partial class MethodEntry
    {
        #region Cloneable

        /// <summary>
        /// Method to create a deep or shallow copy of this object
        /// </summary>
        public MethodEntry Clone(bool deep)
        {
            var copy = new MethodEntry();
            // All value types can be simply copied
            copy.Name = Name; 
            copy.DisplayName = DisplayName; 
            copy.Description = Description; 
            if (deep)
            {
                if (ParameterRoot != null)
                {
                    copy.ParameterRoot = ParameterRoot.Clone(deep);
                }
            }
            else
            {
                // In a shallow clone only references are copied
                copy.ParameterRoot = ParameterRoot; 
            }
            return copy;
        }
        
        #endregion
    }
}
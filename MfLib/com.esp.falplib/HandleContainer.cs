// HandleContainer.cs -*-c#-*-
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace com.esp.falplib
{
    /// <summary>
    /// GCœŠO•Ï”‚ğ•Û‚µ‰ğ•ú‚·‚éˆ×‚ÌƒNƒ‰ƒX
    /// </summary>
    class HandleContainer
    {
        private List<GCHandle> handleList
            = new List<GCHandle>();

        /// <summary>
        /// •Û•Ï”‚Ì’Ç‰Á
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public GCHandle AddPinnedObject(Object obj)
        {
            return GCHandle.Alloc(obj, GCHandleType.Pinned);
        }
        
        /// <summary>
        /// •Û•Ï”‚Ì‰ğ•ú
        /// </summary>
        public void FreeHandle()
        {
            foreach (GCHandle handle in handleList)
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
            handleList.Clear();
        }
    }
}
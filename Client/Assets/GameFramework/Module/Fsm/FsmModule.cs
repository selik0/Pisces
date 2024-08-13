/****************
 *@class name:		FsmModule
 *@description:		
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;

namespace PiscesGame
{
    public class FsmModule : GameModule<FsmModule>
    {
        private Dictionary<TypeNamePair, AbstractFsm> m_fsmDict = new Dictionary<TypeNamePair,AbstractFsm>();
        public override void ReLogin()
        {

        }
    }
}
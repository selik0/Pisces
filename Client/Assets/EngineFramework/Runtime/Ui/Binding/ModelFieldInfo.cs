#if UNITY_EDITOR
/****************
 *@class name:		ModelFieldInfo
 *@description:		ui model字段信息
 *@author:			selik0
*************************************************************************/
using System.Text;
using UnityEngine;
namespace PiscesEngine
{
    public class ModelFieldInfo
    {
        [SerializeField] private int m_dataId;
        [SerializeField] private string m_fieldName;
        [SerializeField] private string m_fieldType;
        public int DataId { get => m_dataId; }
        private ModelFieldInfo() { }
        public ModelFieldInfo(int id)
        {
            m_dataId = id;
        }

        const string FUNC_NAME = "Update{0}";
        const string DATA_DECLARATION = "public {0} {1};";
        public string GetDataDeclarationCode()
        {
            return string.Format(DATA_DECLARATION, m_fieldType, m_fieldName);
        }

        public string GetFuncName()
        {
            return string.Format(FUNC_NAME, m_fieldName.FirstUpper());
        }

        public string GetFuncCode(string codeBody)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine($"public void {GetFuncName()}({m_fieldType} value)");
            code.AppendLine("{");
            code.AppendLine(codeBody);
            code.AppendLine("}");
            return code.ToString();
        }
    }
}
#endif
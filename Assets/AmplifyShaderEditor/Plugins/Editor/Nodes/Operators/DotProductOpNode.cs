// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using UnityEngine;
namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Dot", "Vector Operators", "Scalar dot product of two vectors ( A . B )", null, KeyCode.Period )]
	public sealed class DotProductOpNode : DynamicTypeNode
	{
		static readonly int _Type_PID = Shader.PropertyToID( "_Type" );

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			m_inputPorts[ 0 ].ChangeType( WirePortDataType.FLOAT4, false );
			m_inputPorts[ 1 ].ChangeType( WirePortDataType.FLOAT4, false );
			m_dynamicOutputType = false;
			m_useInternalPortData = true;
			m_allowMatrixCheck = true;
			m_previewShaderGUID = "85f11fd5cb9bb954c8615a45c57a3784";
		}

		public override string BuildResults( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			if ( m_outputPorts[ 0 ].IsLocalValue( dataCollector.PortCategory ) )
				return m_outputPorts[ 0 ].LocalValue( dataCollector.PortCategory );

			base.BuildResults( outputId, ref dataCollector, ignoreLocalvar );
			string result = "dot( " + m_inputA + " , " + m_inputB + " )";
			RegisterLocalVariable( 0, result, ref dataCollector, "dotResult" + OutputId );
			return m_outputPorts[ 0 ].LocalValue( dataCollector.PortCategory );
		}

		private int GetInputPortType( int inputPort )
		{
			int type;
			switch ( m_inputPorts[ 0 ].DataType )
			{
				case WirePortDataType.FLOAT:
				case WirePortDataType.INT:
					type = 1; break;
				case WirePortDataType.FLOAT2:
					type = 2; break;
				case WirePortDataType.FLOAT3:
					type = 3; break;
				case WirePortDataType.FLOAT4:
				default:
					type = 4; break;
			}
			return type;
		}

		public override void SetPreviewInputs()
		{
			base.SetPreviewInputs();
			PreviewMaterial.SetInt( _Type_PID, Mathf.Max( GetInputPortType( 0 ), GetInputPortType( 1 ) ) );
		}
	}
}

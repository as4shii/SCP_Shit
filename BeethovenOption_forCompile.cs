using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//
// Token: 0x0200047F RID: 1151
public class BeethovenOption
{
	// Token: 0x06001A52 RID: 6738 RVA: 0x0001A557 File Offset: 0x00018757
	public BeethovenOption(string variableName, BeethovenOptionType typeOfOption, string optionDestription, string optionCategory)
	{
		this.variable = variableName;
		this.optionType = typeOfOption;
		this.optionName = optionDestription;
		this.category = optionCategory;
		this.cycles = new List<object>();
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x0009F248 File Offset: 0x0009D448
	public string getCurrent()
	{
		switch (this.optionType)
		{
		case BeethovenOptionType.TOGGLE:
			if (!NetworkServer.active)
			{
				return ": <color=#4444FFFF>ENABLED</color>";
			}
			return ": <color=#FF4444FF>DISABLED</color>";
		case BeethovenOptionType.FLOAT:
			return ": <color=#FFFF55FF>disable</color>";
		case BeethovenOptionType.INTEGER:
			return ": <color=#FFFF55FF>aaa</color>";
		default:
			return "";
		}
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x0009F304 File Offset: 0x0009D504
	public object getNextCycle()
	{
		if (this.optionType == BeethovenOptionType.INTEGER)
		{
			int i = 0;
			while (i < this.cycles.Count)
			{
				if (!NetworkServer.active)
				{
					if (i == this.cycles.Count - 1)
					{
						return this.cycles[0];
					}
					return this.cycles[i + 1];
				}
				else
				{
					i++;
				}
			}
		}
		else
		{
			int j = 0;
			while (j < this.cycles.Count)
			{
				if (!NetworkServer.active)
				{
					if (j == this.cycles.Count - 1)
					{
						return this.cycles[0];
					}
					return this.cycles[j + 1];
				}
				else
				{
					j++;
				}
			}
		}
		if (this.cycles != null && this.cycles.Count > 0)
		{
			return this.cycles[0];
		}
		return 1;
	}

	// Token: 0x04001C41 RID: 7233
	public string variable;

	// Token: 0x04001C42 RID: 7234
	public BeethovenOptionType optionType;

	// Token: 0x04001C43 RID: 7235
	public string optionName;

	// Token: 0x04001C44 RID: 7236
	public string category;

	// Token: 0x04001C45 RID: 7237
	private List<object> cycles;
}

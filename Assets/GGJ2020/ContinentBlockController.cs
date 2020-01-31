using System;
using UnityEngine;

public class ContinentBlockController : BaseMonoBehaviour
{
    public enum ResourceType { None, Trees, Water }
    public enum StatusType { None, Fire, Flood }

    [SerializeField] private ResourceType resource;
    public ResourceType Resource => this.resource;
    [SerializeField] private StatusType status;
    public StatusType Status => this.status;
}

﻿using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// 
	/// </summary>
	public interface IManageTiles
	{
		event EventHandler<TileAddedArgs> TileAdded;
		event EventHandler<TileRemovedArgs> TileRemoved;

		void Add(ITile tile);
		void Remove(ITile tile);
	}
}
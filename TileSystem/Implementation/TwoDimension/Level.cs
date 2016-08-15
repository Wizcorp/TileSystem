using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Management;

namespace TileSystem.Implementation.TwoDimension
{
	public class Level : ILevel
	{
		public event EventHandler<AreaAddedArgs> AreaAdded;
		public event EventHandler<AreaRemovedArgs> AreaRemoved;

		public void Add(IArea area)
		{
			throw new NotImplementedException();
		}

		public bool Remove(IArea area)
		{
			throw new NotImplementedException();
		}

		public IArea Get(IPosition position)
		{
			throw new NotImplementedException();
		}

		public List<IArea> GetNeighbours(IArea area)
		{
			throw new NotImplementedException();
		}
	}
}

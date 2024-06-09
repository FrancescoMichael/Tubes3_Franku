namespace FrankuGUI{
    class DBSCAN
    {
		public int eps;
		public int minPts;
		public List<Point> data;  // supplied in cluster()
		public List<int> labels;  // supplied in cluster()
		public List<string> baseList;
		public List<string> queryList;
		public List<List<int>> indexList;
		public int M;
		public int N;

		public DBSCAN(int eps, int minPts)
		{
			this.eps = eps;
			this.minPts = minPts;
			this.labels = new List<int>();
			this.M = 0;
			this.N = 0;
			this.baseList = new List<string>();
			this.queryList = new List<string>();
			this.indexList = new List<List<int>>();
			this.data = new List<Point>();
			this.labels = new List<int>();
		}  

		public void changeProp(List<string> baseList, List<string> queryList, List<List<int>> indexList)
		{
			this.baseList = baseList;
			this.queryList = queryList;
			this.indexList = indexList;
			this.N = baseList.Count;
			this.M = baseList[0].Length;
		}

		public List<int> Cluster(List<Point> data)
		{
			this.data = data;  // by reference
			
			if(this.labels.Count != this.data.Count){
				this.labels = new List<int>(this.data.Count);
				for(int i = 0; i < this.data.Count; i++)
				{
					this.labels.Add(-2);
				}
			}
			else
			{
				for(int i = 0; i < this.data.Count; i++){
					this.labels[i] = -2;
				}
			}
			// unprocessed

			int cid = -1;  // offset the start
			for (int i = 0; i < this.data.Count; ++i)
			{
				if (this.labels[i] != -2)  // has been processed
					continue;

				List<int> neighbors = this.RegionQuery(i);
				
				if (neighbors.Count < this.minPts)
				{
					this.labels[i] = -1;  // noise
				}
				else
				{
					++cid;
					this.Expand(i, neighbors, cid);
				}
			}

			return this.labels;
		}

		private List<int> RegionQuery(int p)
		{
			List<int> result = new List<int>();
			Point askedPoint = this.data[p];
			int xPoint = askedPoint.X;
			int yPoint = askedPoint.Y;
			for(int i = Math.Max(0, xPoint - eps); i <= Math.Min(N - 1, xPoint + eps); i++)
			{
				for(int j = Math.Max(0, yPoint - eps); j <= Math.Min(M - 1, yPoint + eps); j++)
				{
					int uwu = (i - xPoint) * (i - xPoint) + (j - yPoint) * (j - yPoint);
					if(uwu > eps * eps) continue;
					if(baseList[i][j] != queryList[i][j])
					{
						result.Add(this.indexList[i][j]);
					}
				}
			}
			return result;
		}

		private void Expand(int p, List<int> neighbors, int cid)
		{
			this.labels[p] = cid;
			for (int i = 0; i < neighbors.Count; ++i)
			{
				int pn = neighbors[i];
				
				if (this.labels[pn] == -1)  // noise
					this.labels[pn] = cid;
				else if (this.labels[pn] == -2)  // unprocessed
				{
					this.labels[pn] = cid;
					List<int> newNeighbors = this.RegionQuery(pn);
					if (newNeighbors.Count >= this.minPts)
						neighbors.AddRange(newNeighbors); // modifies loop
				}
			}
		}

		private static double EucDistance(double[] x1, double[] x2)
		{
			int dim = x1.Length;
			double sum = 0.0;
			for (int i = 0; i < dim; ++i)
			sum += (x1[i] - x2[i]) * (x1[i] - x2[i]);
			return Math.Sqrt(sum);
		}
    }
}
namespace mergeblox
{
	internal class MoveInfo
	{
		private int _score;
		private bool _shifted;

		public int Score { get => _score; set => _score = value; }
		public bool Shifted { get => _shifted; set => _shifted = value; }

		public MoveInfo()
		{
			_score = 0;
			_shifted = false;
		}
	}
}

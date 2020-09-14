namespace AgPath
{
    public class Node
    {
        public bool Obstacle { get; set; }
        public bool Visited { get; set; }
        public Position Position { get; set; }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}

using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Problem4
    {
        public static int SolveA(List<string> input)
        {
            var totalPoints = 0;
            foreach(var item in input)
            {
                var card = new Card(item);
                totalPoints += card.CalculatePoints();
            }

            return totalPoints;
        }

        //works but it's waaaay too slow, must re-approach
        public static int SolveB(List<string> input)
        {
            List<Card> cards = new List<Card>();
            foreach (var item in input)
            {
                cards.Add(new Card(item));
            }

            int cardTotal = 0;

            foreach (var card in cards)
            {
                for(int j = 0; j < card.Repetitions; j++)
                {
                    for (int i = 1; i <= card.AmountOfMatches; i++)
                    {
                        var targetId = card.Id + i;
                        if (targetId <= cards.Count)
                            cards.First(c => c.Id == card.Id + i).Repetitions++;
                    }
                }
                
                cardTotal += card.Repetitions;
            }

            return cardTotal;
        }

        private class Card
        {
            public int Id;
            public List<int> WinningNumbers { get; set; } = [];
            public List<int> PlayedNumbers { get; set; } = [];

            public int Repetitions { get; set; } = 1;

            public int AmountOfMatches => PlayedNumbers.Count(p => WinningNumbers.Any(w => w == p));

            public Card(string cardString)
            {
                var title = cardString.Split(':')[0];
                var winningNumbersString = cardString.Split(":")[1].Split("|")[0];
                var playedNumbersString = cardString.Split(":")[1].Split("|")[1];

                string pattern = @"(\d+):";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(cardString);
                Id = int.Parse(match.Groups[1].Value);

                pattern = @"\d+";
                regex = new Regex(pattern);

                var winningMatches = regex.Matches(winningNumbersString);
                foreach(Match w in winningMatches)
                {
                    WinningNumbers.Add(int.Parse(w.Value));
                }

                var playedMatches = regex.Matches(playedNumbersString);
                foreach (Match p in playedMatches)
                {
                    PlayedNumbers.Add(int.Parse(p.Value));
                }
            }

            public int CalculatePoints()
            {
                var points = 0;
                if(AmountOfMatches == 0) return points;
                for(int i = 0; i < AmountOfMatches; i++)
                {
                    points = points == 0 ? points += 1 : points *= 2;
                }
                return points;
            }
        }
    }
}

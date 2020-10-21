using System;
using System.Collections.Generic;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private char[] separators = new[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        private Dictionary<string, Dictionary<int, HashSet<int>>> words;

        public void Add(int id, string documentText)
        {
            var document = documentText.Split(separators);
            int currentPosition = 0;

            foreach (var word in document)
            {
                AddtoWords(word, id, currentPosition);
                currentPosition += word.Length + 1;
            }
        }

        private void AddtoWords(string word, int id, int currentPosition)
        {
            if (!words.ContainsKey(word))
            {
                var wordPositions = new HashSet<int>();
                wordPositions.Add(currentPosition);
                words.Add(word, new Dictionary<int, HashSet<int>>());
                words[word].Add(id, wordPositions);
            }
            else if (!words[word].ContainsKey(id))
            {
                words[word].Add(id, new HashSet<int>());
                words[word][id].Add(currentPosition);
            }
            else
            {
                words[word][id].Add(currentPosition);
            }
        }

        public List<int> GetIds(string word)
        {
            if (words.ContainsKey(word))
                return new List<int>(words[word].Keys);
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (words.ContainsKey(word) && words[word].ContainsKey(id))
                return new List<int>(words[word][id]);
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var word in words.Keys)
                if (words[word].ContainsKey(id))
                    words[word].Remove(id);
        }

        public Indexer()
        {
            words = new Dictionary<string, Dictionary<int, HashSet<int>>>();
        }
    }
}

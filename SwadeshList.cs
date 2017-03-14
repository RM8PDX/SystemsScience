using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SwadeshList : MonoBehaviour {
    const string sounds = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
    public int minMuts = 1;
    public int maxMuts = 5;
    public int minWordLen = 2;
    public int maxWordLen = 16;

    public TextAsset vocab;

    public List<Phoneme> phonemes = new List<Phoneme>();
    public Dictionary<string, List<Phoneme>> swadishDict = new Dictionary<string, List<Phoneme>>();


    public void GrabSettings() {
        SettingsSetter settings = FindObjectOfType<SettingsSetter>();

        minWordLen = settings.minWordLength;
        maxWordLen = settings.maxWordLength;
    }


    public void Start() {
        GeneratePhonemes();
        PopulateSwadishDict();
    }


    public void PopulateSwadishDict() {
        foreach (string meaning in VocabWords())
            if (!swadishDict.ContainsKey(meaning))
                swadishDict.Add(meaning, RndPronunciation());
    }


    /// <summary>
    ///  Generate a Mutation struct which contains an (already generated)
    ///  phoneme and the probability that the given mutation will occur.
    /// </summary>
    public Mutation RndMut() {
        return new Mutation() {
            target = phonemes[Random.Range(0, phonemes.Count - 1)],
            prob   = Random.Range(0.0f, 1.0f)
        };
    }


    /// <summary>
    /// Populate our phoneme list which indicates the glyph representing
    /// our phoneme and which other phonemes it can turn into if the
    /// pronunciation gets too out of whack.
    /// </summary>
    public void GeneratePhonemes() {
        for (int i = 0; i < sounds.Length; i++)
            phonemes.Add(new Phoneme(sounds[i]));

        // Now fill in the transition/mutation probabilities.
        foreach (Phoneme p in phonemes) {

            // Add a random number (up to 5) of mutations for high value pronunciations.
            for (int i = 0; i < Random.Range(minMuts, maxMuts); i++)
                p.highValueMutations.Add(RndMut());

            // Do the exact same thing as above, but for low value pronunciations.
            for (int i = 0; i < Random.Range(minMuts, maxMuts); i++)
                p.lowValueMutations.Add(RndMut());
        }
    }


    /// <summary>
    /// Create a random list of phonemes which indicate
    /// how a word should is pronounced.
    /// </summary>
    /// <returns>
    /// A list of phonemes representing the phonetic pronunciation of a word.
    /// </returns>
    public List<Phoneme> RndPronunciation() {
        // A word is a list of phonemes.
        List<Phoneme> word = new List<Phoneme>();
        for (int i = 0; i < Random.Range(minWordLen, maxWordLen); i++) {
            word.Add(phonemes[Random.Range(0, phonemes.Count - 1)]);
        }
        return word;
    }


    /// <summary>
    /// Yields every (English) word in our Swadish list that will need
    /// a pronunciation for in a new language.
    /// </summary>
    /// <returns>A swadesh vocabulary word like "one" or "teeth."</returns>
    public IEnumerable<string> VocabWords() {
        string txt = vocab.text;
        char[] delimiters = { '\n' };
        foreach (string word in txt.Split(delimiters)) {
            yield return word;
        }
    }
}

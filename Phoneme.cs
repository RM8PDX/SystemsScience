using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// When a given pronunciation becomes extreme, it
// will transition into a new phoneme with some
// probability. Since Unity doesn't support Tuples,
// we use a struct to represent that pair of values.
public struct Mutation {
    public Phoneme target;
    public float prob;
}


// For simplicity's sake, we ignore the actual biological
// pronunciation details and instead just assume use an
// arbitrary letter that varies one some single, continuous
// axis (the exact position along that axis is stored by the
// agent in question, whereas this structure represents only
// the phoneme and it's transitions to other sounds, not its
// phonemic realization).
public class Phoneme {
    public char glyph;
    public List<Mutation> highValueMutations = new List<Mutation>();
    public List<Mutation> lowValueMutations  = new List<Mutation>();

    public Phoneme(char c) {
        glyph = c;
    }
}


//public class PhonemeVariant {
//    public Phoneme phoneme;
//    public float value;
//}

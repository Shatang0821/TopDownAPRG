using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;


public class MixAnimationSample : MonoBehaviour
{
    public AnimationClip clip1,clip2;
    [Range(0f,1f)]
    public float weight;

    private AnimationMixerPlayable mixer;
    PlayableGraph graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = PlayableGraph.Create();
        mixer = AnimationMixerPlayable.Create(graph, 2);
        
        
        //graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        var clip1Playable = AnimationClipPlayable.Create(graph, clip1);
        var clip2Playable = AnimationClipPlayable.Create(graph, clip2);
        graph.Connect(clip1Playable, 0, mixer, 0);
        graph.Connect(clip2Playable, 0, mixer, 1);
        
        mixer.SetInputWeight(0,0.5f);
        mixer.SetInputWeight(1,0.5f);
        var output = AnimationPlayableOutput.Create(graph, "Anim", GetComponent<Animator>());
        output.SetSourcePlayable(mixer);
        
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
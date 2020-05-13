using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
 
[RequireComponent(typeof(VideoPlayer))]
public class UIVideoImage : MonoBehaviour
{
    private RenderTexture movie;
    private Image image;
    private RawImage rawImage;
    private VideoPlayer player;
    public UIMode UGUI;

    public enum UIMode
    {
        None = 0,
        Image = 1,
        RawImage = 2
    }
    // Use this for initialization
    void Start()
    {
        player = GetComponent<VideoPlayer> ();
        player.sendFrameReadyEvents = true;
        player.loopPointReached += OnVideoLoopOrPlayFinished;
        if (UGUI == UIMode.Image)
        {
            movie = new RenderTexture((int)player.clip.width, (int)player.clip.height, 24);
            image = GetComponent<Image> ();
            image.color = new Color(1, 1, 1, 0);
            player.renderMode = VideoRenderMode.RenderTexture;
            player.targetTexture = movie;
        }
        else if (UGUI == UIMode.RawImage)
        {
            rawImage = GetComponent<RawImage> ();
            rawImage.color = new Color(1, 1, 1, 0);
            player.renderMode = VideoRenderMode.APIOnly;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (UGUI == UIMode.Image)
        {
            //在Image上播放视频
            if (!player.isPlaying) return;
            if (player.targetTexture == null) return;
            int width = player.targetTexture.width;
            int height = player.targetTexture.height;
            Texture2D t = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture.active = player.targetTexture;
            t.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            t.Apply();
            image.sprite = Sprite.Create(t, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f)) as Sprite;
            image.SetNativeSize();
            if (image.color.a == 0)
            {
                image.color = Color.white;
            }
        }
        if (UGUI == UIMode.RawImage)
        {
            //在RawImage上播放视频
            if (rawImage.texture == null)
            {
                if (player.texture == null) return;
                rawImage.texture = player.texture;
                rawImage.SetNativeSize();
                rawImage.color = Color.white;
            }
        }
    }
    // 播放完成或者循环完成事件
    public void OnVideoLoopOrPlayFinished(VideoPlayer vp)
    {
        transform.parent.GetComponent<StoryVedio>().VedioEnd();
    }
}
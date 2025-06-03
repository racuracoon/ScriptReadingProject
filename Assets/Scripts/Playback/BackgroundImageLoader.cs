using UnityEngine;
using SFB;
using System.IO;
using TMPro;

public class BackgroundImageLoader : MonoBehaviour
{
    public Renderer wallRenderer;           // 벽 오브젝트의 MeshRenderer

    public string LoadImage()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("이미지 선택", "", extensions, false); if (paths.Length > 0 && File.Exists(paths[0]))
        {
            return paths[0];
        }
        else
        {
            return null;
        }
    }

    public bool ApplyImage(string path)
    {
        bool exits = File.Exists(path);
        if (!exits)
        {
            Debug.Log("없음");
            Debug.Log(path);
        }
        if (!File.Exists(path))
        {
            Debug.LogWarning("❌ 해당 경로에 파일이 없습니다.");
            return false;
        }

        byte[] imageData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageData))
        {
            wallRenderer.material.mainTexture = texture;
            Debug.Log("✅ 벽에 이미지 적용 완료");
            return true;
        }
        else
        {
            Debug.LogWarning("❌ 이미지 로드 실패");
            return false;
        }
    }
}

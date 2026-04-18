using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BookScript : MonoBehaviour
{
    public enum ContentType { IMAGES, TEXT };
    public ContentType contentType;
    public Sprite [] pageContents;
    public GameObject leftPage;
    public GameObject rightPage;
    public int numberOfPagesToIncrement;
    private int cursor = 0;
    public enum PageDirection { FORWARD, BACKWARD, STATIC };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setPages(PageDirection.STATIC, contentType);
    }

    private float timer = 0f;
    void Update()
    {
        //Debug
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            setPages(PageDirection.FORWARD, contentType);
            timer = 0;
        }
    }

    public void setPages(PageDirection dir, ContentType type)
    {
        switch (dir)
        {
            case PageDirection.FORWARD:
                cursor = (cursor + numberOfPagesToIncrement) < pageContents.Length - 1 ? cursor + numberOfPagesToIncrement : cursor;
                break;
            case PageDirection.BACKWARD:
                cursor = (cursor - numberOfPagesToIncrement) > -1 ? cursor - numberOfPagesToIncrement : cursor;
                break;
            case PageDirection.STATIC:
                break;
        }

        switch (type)
        {
            case (ContentType.IMAGES):
                if (leftPage != null) leftPage.GetComponent<Image>().sprite = pageContents[cursor];
                if (rightPage != null) rightPage.GetComponent<Image>().sprite = pageContents[cursor + 1];
                break;
            case (ContentType.TEXT):

                //////////
                //TODO CLIPBOARD PAGE TURNING LOGIC HERE//
                //////////
                ///
                break;
        }
        
    }
}

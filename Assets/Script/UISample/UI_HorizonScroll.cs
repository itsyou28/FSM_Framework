using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


/// <summary>
/// 뷰포트 중앙에 가장 가까운 아이템에 자동으로 Snap 시켜주는 기능을 한다. 
/// 스크롤 컴포넌트가 있는 오브젝트에 붙여야 한다. 
/// Horizon Scroll 하위 Content 패널에 아이템을 배치한다. 
/// 아이템의 개수 및 너비는 필요에 따라 조정한다. 
/// 아이템의 너비는 모두 동일해야 한다. 
/// 아이템 및 Content 패널의 x축 Pivot은 0.5로 맞춰야 한다. 
/// </summary>
public class UI_HorizonScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    ScrollRect _scroll;
    [SerializeField]
    RectTransform _viewport;
    
    float[] arrSnapPos;
        
    float normalizedItemWidth;

    int curIDX;

    bool wasDrag = false;
    bool isSnap = false;
    float dampVelocity;
    float dampResult;
    const float dampStartSpeed = 0.5f;
        
    //프리팹 로딩시 레이아웃 처리가 비동기로 이뤄진다. 
    //정확한 초기화를 위해 한 프레임 대기한다. 
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        float contentPanelWidth = _scroll.content.GetComponent<RectTransform>().rect.width;
        
        //스크롤 되는 영역은 컨텐츠 패널의 전체 길이에서 뷰포트 길이만큼을 제외한 영역이다. 
        float scrollAreaWidth = contentPanelWidth - _viewport.rect.width;

        //스크롤 영역 내에서 아이템 너비가 차지하는 비율을 구한다. 
        normalizedItemWidth = _scroll.content.GetChild(0).GetComponent<RectTransform>().rect.width / scrollAreaWidth;

        //피봇을 center Position으로 맞출 경우 SnapPoint를 계산할 때 뷰포트 영역의 절반만큼 빼고 계산해야 한다. 
        float halfViewport = _viewport.rect.width * 0.5f;

        //content 하위에 있는 노드를 검색해서 snap포인트를 계산한다. 
        arrSnapPos = new float[_scroll.content.transform.childCount];
        for (int i = 0; i < _scroll.content.transform.childCount; i++)
        {
            RectTransform t = _scroll.content.transform.GetChild(i).GetComponent<RectTransform>();
            arrSnapPos[i] = (t.anchoredPosition.x - halfViewport) / scrollAreaWidth;
        }

        //가운데 아이템 위치에서 스크롤이 시작하도록 인덱스를 지정한다. 
        curIDX = Mathf.FloorToInt(_scroll.content.childCount * 0.5f);
        wasDrag = true;
    }

    void Update()
    {
        if (wasDrag && Mathf.Abs(_scroll.velocity.x) < 100.0f)
        {
            wasDrag = false;
            _scroll.velocity = Vector2.zero;

            //정규화된 포지션값과 아이템 너비값을 이용해 현재 스크롤 위치랑 가장 가까운 아이템 인덱스를 구한다. 
            curIDX = Mathf.RoundToInt(_scroll.horizontalNormalizedPosition / normalizedItemWidth);

            if (curIDX > _scroll.content.childCount)
            {
                UDL.LogError("인덱스 초과. \n 초기화가 정상적으로 이뤄졌는지 확인해주세요. \n 아이템의 너비가 모두 동일한지 확인해주세요. ");
                return;
            }

            if (_scroll.horizontalNormalizedPosition > arrSnapPos[curIDX])
                dampVelocity = -dampStartSpeed;
            else
                dampVelocity = dampStartSpeed;

            isSnap = true;
        }

        if (isSnap)
            Snap();
    }

    //Damp를 이용해 지정된 SnapPoint로 Snap시킨다. 
    void Snap()
    {
        dampResult = Mathf.SmoothDamp(
            _scroll.horizontalNormalizedPosition, arrSnapPos[curIDX], ref dampVelocity, 1);

        _scroll.horizontalNormalizedPosition = dampResult;

        //오차범위 이내로 가까워지면 Snap 액션을 중지한다. 
        if (Mathf.Abs(_scroll.horizontalNormalizedPosition - arrSnapPos[curIDX]) < 0.001f)
            isSnap = false;
    }

    public void OnBeginDrag(PointerEventData _data)
    {
        isSnap = false;
    }

    public void OnEndDrag(PointerEventData _data)
    {
        wasDrag = true;
    }
}

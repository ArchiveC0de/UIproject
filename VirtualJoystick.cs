using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;  
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)] // Rage어브리뷰트(유니티내 설정, 10 150)
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput; // 조건역할로 사용되는 변수

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) //  Drag Start 
    {
        ConstrolJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData) // Draging => 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트 
    {                                              // 이떄 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않는다.
        ConstrolJoystickLever(eventData);          // 즉, 드래그 도중 마우스를 멈추고 있으면 이벤트가 들어오지 않는다. 
    }

    public void OnEndDrag(PointerEventData eventData) // Drag End
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    private void ConstrolJoystickLever(PointerEventData eventData) // 반복되는 이벤트를 함수로 정의해서 구현 => 호출해준다.
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange; // inputVector은 해상도를 기반으로 만들어진값으로 그대로 캐릭터의 이동속도로 사용하기엔 너무 큰값을 갖는다.
                                                   // 해상도를 기준으로 하였기에 해상도가 바뀌면 이동방향도 달라진다. 때문에 정규화된 수치에서 사용해야됨 
    }

    private void InputControlVector()
    {
        // 캐릭터에게 입력백터를 전달
        Debug.Log(inputDirection.x + "/" + inputDirection.y);
    }

    void Update() // OnbeginDrag, Ondrag에서 처리할 수 있을탠대 굳이 Update에서 정의해서 처리하는이유 
    {             // Ondrag 함수의 특성과 관련이 있다. => 연속적으로 호출하기위해서 Update에서 정의하였다.
        InputControlVector();
    }
}

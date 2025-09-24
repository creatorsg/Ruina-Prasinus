using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : EventPageBase
{
    private void Awake()
    {
        SetTrigger(TriggerType.ActionButton);
        SetConditions(new Dictionary<string, bool>() { });
    }

    override protected IEnumerator Contents()
    {
        EnterActor("정원사", DirectionType.Left, "romanStanding_Embrass");
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "기관차...? 방금 폭발 소리, 이거였나?"));

        EnterActor("기관사", DirectionType.Right, "engineerStanding_Blink");
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "후이잇!! ㅈ... 잡아먹지 마세요오!!"));

        UpdateActor("정원사", "romanStanding_Idle");
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "... 잡아먹긴 누가 잡아먹습니까."));

        UpdateActor("기관사", "engineerStanding_Idle");
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "ㅇ... 우왓... 여기도 생존자가..."));

        UpdateActor("정원사", "romanStanding_Think");
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "본론으로 돌아와서... 이 기관차가 폭발한 건가요?"));

        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "ㄴ... 네에... 터졌어요오..."));
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "기름 탱크가 아니라 연료밸브 쪽이..."));
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "이거, 편도로는 움직일 수는 있긴 한데에... 문제는..."));

        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "안에 다른 누가 있었다던가? 흔적이 이상한데요."));

        UpdateActor("정원사", "romanStanding_Embrass");
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "...뭐에요, 이 소리...?"));

        UpdateActor("기관사", "engineerStanding_Blink");
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "...ㅈ... 전 말했어요오...! 안... 안쪽이 뭔가 이상하다고...!"));

        UpdateActor("기관사", "engineerStanding_Idle");
        yield return StartCoroutine(ShowText(
            "기관사",
            DirectionType.Right,
            "후히... ㅇ... 이제는 정말 싸워야 하는 건가요오..."));

        UpdateActor("정원사", "romanStanding_Idle");
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "어짜피 저 녀석에게 말 걸어봐야 소용없겠군요."));
        yield return StartCoroutine(ShowText(
            "정원사",
            DirectionType.Left,
            "좋아, 간단하게 끝내자구."));
        HideText();
    }

    private void SetTrigger(TriggerType triggerType) =>
        _eventPageTrigger = new EventPageTrigger(triggerType);

    private void SetConditions(Dictionary<string, bool> switchOperation) =>
        _eventPageCondition = new EventPageCondition(switchOperation);
}

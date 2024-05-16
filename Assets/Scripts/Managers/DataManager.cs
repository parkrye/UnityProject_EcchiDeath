using System.Linq;
using UnityEngine;

public class DataManager : BaseManager
{
    public PlayData PlayData { get; private set; }
    public EventData[] Events { get; private set; }
    public Sprite[] Sprites { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        Application.runInBackground = false;

        AddJudgeElements();

        PlayData = GameManager.Resource.Load<PlayData>("PlayData");
        Events = GameManager.Resource.LoadAll<EventData>("Events").OrderBy(t => t.name).ToArray();
        Sprites = GameManager.Resource.LoadAll<Sprite>("Sprites/NPCs").OrderBy(t => t.name).ToArray();
    }

    public void AddPlayerData(int date = 0, int passCount = 0, int deathCount = 0, int score = 0)
    {
        PlayData.Date += date;
        PlayData.PassCount += passCount;
        PlayData.DeathCount += deathCount;
        PlayData.Score += score;
    }

    public void SetPlayerData(int date = 0, int passCount = 0, int deathCount = 0, int score = 0)
    {
        PlayData.Date = date;
        PlayData.PassCount = passCount;
        PlayData.DeathCount = deathCount;
        PlayData.Score = score;
    }

    public void ResetData()
    {
        PlayData.Date = 0;
        PlayData.PassCount = 0;
        PlayData.DeathCount = 0;
        PlayData.Score = 0;
    }

    private void AddJudgeElements()
    {
        var haveElements = new (string name, int value)[]
        {
            ("네브라 디스크", 0), ("파에스토스 원반", 0), ("볼프세크 강철", 0), ("님루드 렌즈", 0), ("만드라고라 농축액", 0),
            ("로혼치 사본", 0), ("에테르 정수", 0), ("안티키테라 장치", 0), ("보이니치 사본", 0), ("수정 하니와", 1),
            ("토템폴", 0), ("고대 전지", 0), ("디스코 콜간테", 0), ("위니페소키 스톤", 0), ("머리가 자라는 인형", 0),
            ("아틀란티스 메달", 0), ("황금 드레스", 0), ("로마 12면체", 0), ("킴바야 유물", 0), ("이스탄불 로켓", 0),
            ("시한 폭탄", 0), ("교양 사격 WB", 0), ("여분의 속옷", 0), ("학교 수영복", 0), ("타 학교 교복", 0),

            ("에로 동인지", 1), ("교양 체육 WB", 1), ("교양 위생 WB", 1), ("에너지 드링크", 1), ("쮸쮸바", 1), ("H 연필", 1), 
            ("개 목줄", 1), ("니플 밴드", 1), ("성교육 동화", 1), ("타 학교 수영복", 1),
            ("에로 비디오", 2), ("에로 게임 디스크", 2), ("선생님의 수영복", 2),  ("타인의 수영복", 2), ("타인의 속옷", 2), 
            ("성인 잡지", 2),
            ("금단의 연애 ~ 허락되지 않아서, 더욱 아름다운 사랑~", 5), ("유스티나 성도회 예장", 5), ("선생님의 속옷", 5),

            ("기초 전술 교육 BD", -1), ("기초 기술 노트", -1),
            ("일반 전술 교육 BD", -2), ("일반 기술 노트", -2),
            ("상급 전술 교육 BD", -3), ("상급 기술 노트", -3),
            ("최상급 전술 교육 BD", -4), ("최상급 기술 노트", -4),
            ("성경", -5),
        };

        foreach(var have in haveElements)
        {
            GameData.JudgeElements.Add(new JudgeElement("{0} 소지", have.value, have.name));
        }

        var wearElements = new (string name, int value)[]
        {
            ("트리니티 지정 교복", 0), ("타 학교 교복", 0),

            ("학교 수영복", 2), ("타인의 수영복", 2), ("타 학교 수영복", 2),
            ("비키니 수영복", 3), ("여분의 속옷", 3), ("타인의 속옷", 3),
            ("유스티나 성도회 예장", 5), ("선생님의 수영복", 5),
            ("선생님의 속옷", 10),

            ("정의실현부 교복", -1), ("시스터후드 교복", -1),
        };

        foreach (var wear in wearElements)
        {
            GameData.JudgeElements.Add(new JudgeElement("{0} 착의", wear.value, wear.name));
        }

        var actionElements = new (string name, int value)[]
        {
            ("수업 지각", 0), ("성적 부진", 0), ("무단 외출", 0), ("타인 학습 방해", 0), ("규정 외 화장품 사용", 0),
            ("타인 취침 방해", 0), ("보행 취식", 0), ("품위 없는 행위", 0), ("숙사 내 동물 사육", 0),
            ("숙사 내 음식 반입", 0), ("수업 결석", 0), ("블랙마켓 출입", 0),

            ("무단 외박", 1), ("유흥업소 출입", 1), ("불건전한 교제", 1), ("음란물 소지", 1), ("음란물 사용", 1),
            ("공개된 장소에서 노출", 1), ("불건전한 언어 사용", 1),
            
            ("학내 봉사활동", -1), ("학외 봉사활동", -1),
        };

        foreach (var action in actionElements)
        {
            GameData.JudgeElements.Add(new JudgeElement("{0} 기록", action.value, action.name));
        }
    }
}
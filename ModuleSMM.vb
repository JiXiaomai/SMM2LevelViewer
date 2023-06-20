Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices

Module ModuleSMM
    Public Const UR As String = "198174193181024108188204033027038009110065067055059092053071110013197170026126044073185151163192252147231138210253130239234135026040040007108000240149029107071034164200170245039067011106201189022119240223"
    Public MapWidth(1), MapHeight(1) As Integer
    Public LvlData() As Byte
    Public Structure LvlHeader
        Dim StartY As Byte
        Dim GoalY As Byte
        Dim GoalX As Int16
        Dim Timer As Int16
        Dim ClearCA As Int16
        Dim DateYY As Int16
        Dim DateMM As Byte
        Dim DateDD As Byte
        Dim DateH As Byte
        Dim DateM As Byte
        Dim AutoscrollSpd As Byte
        Dim ClearCC As Byte
        Dim ClearCRC As Integer
        Dim GameVer As Integer
        Dim MFlag As Integer
        Dim ClearAttempts As Integer
        Dim ClearTime As Integer
        Dim CreationID As Integer
        Dim UploadID As Long
        Dim ClearVer As Integer
        Dim GameStyle As Int16
        Dim Name As String
        Dim Desc As String
    End Structure
    '关卡文件头H200
    Public LH As LvlHeader, PZoom As Integer = 16
    Public Structure MapObject
        Dim X As Integer
        Dim Y As Integer
        Dim W As Byte
        Dim H As Byte
        Dim Flag As Integer
        Dim CFlag As Integer
        Dim Ex As Integer
        Dim ID As Int16
        Dim CID As Int16
        Dim LID As Int16
        Dim SID As Int16
        Dim LinkType As Byte
    End Structure
    Public Structure MapGround
        Dim X As Byte
        Dim Y As Byte
        Dim ID As Byte
        Dim BID As Byte
    End Structure
    Public Structure MapHeader
        Dim Theme As Byte
        Dim AutoscrollType As Byte
        Dim BorFlag As Byte
        Dim Ori As Byte
        Dim LiqEHeight As Byte
        Dim LiqMode As Byte
        Dim LiqSpd As Byte
        Dim LiqSHeight As Byte
        Dim BorR As Integer
        Dim BorT As Integer
        Dim BorL As Integer
        Dim BorB As Integer
        Dim Flag As Integer
        Dim ObjCount As Integer
        Dim SndCount As Integer
        Dim SnakeCount As Integer
        Dim ClearPipCount As Integer
        Dim CreeperCount As Integer
        Dim iBlkCount As Integer
        Dim TrackBlkCount As Integer
        Dim GroundCount As Integer
        Dim TrackCount As Integer
        Dim IceCount As Integer
    End Structure
    Public Structure MapTrack
        Dim UN As Short
        Dim Flag As Byte
        Dim X As Byte
        Dim Y As Byte
        Dim Type As Byte
        Dim LID As Short
        Dim K0 As Integer
        Dim K1 As Integer
        Dim F0 As Byte
        Dim F1 As Byte
        Dim F2 As Byte
    End Structure
    Public Structure MapClearPipe
        Dim Index As Byte
        Dim NodeCount As Byte
        Dim Node() As MapClearPipeNode
    End Structure
    Public Structure MapClearPipeNode
        Dim type As Byte
        Dim index As Byte
        Dim X As Byte
        Dim Y As Byte
        Dim W As Byte
        Dim H As Byte
        Dim Dir As Byte
    End Structure
    Public Structure MapSnakeBlock
        Dim index As Byte
        Dim NodeCount As Byte
        Dim Node() As MapSnakeBlockNode
    End Structure
    Public Structure MapSnakeBlockNode
        Dim index As Byte
        Dim Dir As Byte
    End Structure
    Public Structure MapMoveBlock
        Dim index As Byte
        Dim NodeCount As Short
        Dim Node() As MapMoveBlockNode
    End Structure
    Public Structure MapMoveBlockNode
        Dim p0 As Byte
        Dim p1 As Byte
        Dim p2 As Byte
    End Structure
    Public Structure MapCreeper
        Dim index As Byte
        Dim NodeCount As Short
        Dim Node() As Byte
    End Structure
    Public Structure DeathData
        Dim x As Integer
        Dim y As Integer
        Dim Map As Boolean
    End Structure
    Public MapCPipe(1, 200) As MapClearPipe
    Public MapSnk(1, 5) As MapSnakeBlock, OSNK As Integer
    Public TrackNode(1, 300, 300), GroundNode(1, 300, 300) As Integer
    Public TrackYPt(15, 2) As Point
    Public MapMoveBlk(1, 10), MapTrackBlk(1, 10) As MapMoveBlock
    Public MapCrp(1, 10) As MapCreeper
    Public ObjLinkType(1, 60000) As Byte
    Public MH(1) As MapHeader, MapHdr As MapHeader
    Public MapObj(1, 2600) As MapObject
    Public MapGrd(1, 4000), MapIce(1, 300), MapSnd(1, 300) As MapGround
    Public MapTrk(1, 1500) As MapTrack
    Public MapDeath() As DeathData
    Public ObjEng() As String = {"栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币",
    "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW",
    "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖",
    "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头",
    "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "大蘑菇", "鞋子栗宝宝", "碎碎龟", "加农炮", "鱿鱿", "城堡桥",
    "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡",
    "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台",
    "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "USA蘑菇", "圈圈", "狼牙棒",
    "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币",
    "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记",
    "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖",
    "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿",
    "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床"}
    Public ObjEng3() As String = {"栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币",
    "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW",
    "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖",
    "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头",
    "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "超级树叶", "鞋子栗宝宝", "碎碎龟", "加农炮", "鱿鱿", "城堡桥",
    "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡",
    "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台",
    "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "青蛙装", "圈圈", "狼牙棒",
    "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币",
    "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记",
    "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖",
    "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿",
    "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床"}
    Public ObjEngW() As String = {"栗邦邦", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币",
    "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW",
    "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖",
    "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头",
    "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "斗篷羽毛", "耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥",
    "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡",
    "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台",
    "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "力量气球", "圈圈", "狼牙棒",
    "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币",
    "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记",
    "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖",
    "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿",
    "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床"}
    Public ObjEngU() As String = {"栗宝宝", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币",
    "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW",
    "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖",
    "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头",
    "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "螺旋桨蘑菇", "耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥",
    "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡",
    "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台",
    "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "超级橡栗", "圈圈", "狼牙棒",
    "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币",
    "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记",
    "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖",
    "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿",
    "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床"}
    Public ObjEngD() As String = {"板栗", "慢慢龟", "吞食花", "铁锤兄弟", "软砖", "？砖", "硬砖", "地面", "金币",
    "水管", "弹簧", "升降台", "咚咚", "炮台", "蘑菇平台", "炸弹兵", "半碰撞平台", "桥", "P开关", "POW",
    "蘑菇", "竹轮", "云砖", "音符", "火焰棒", "刺刺龟", "终点砖", "终点旗", "钢盔龟", "隐藏砖",
    "球盖姆", "球盖姆云", "炮弹刺客", "1UP蘑菇", "火焰花", "无敌星", "熔岩台", "起点砖", "起点箭头",
    "魔法师", "尖刺龟", "幽灵", "小丑飞船", "刺", "超级铃铛", "鞋/耀西", "碎碎龟", "加农炮", "鱿鱿", "城堡桥",
    "跳跳机", "跳跳鼠", "花毛毛", "传送带", "喷枪", "门", "泡泡鱼", "黑花", "扳手仔", "轨道", "火焰泡泡",
    "汪汪", "库巴", "冰砖", "树藤", "叮叮蜂", "箭头", "单向板", "圆锯", "Player", "10/30/50金币", "半碰撞平台",
    "慢慢龟汽车", "奇诺比奥", "加邦/铁球", "石头", "龙卷风", "奔奔", "仙人掌", "P砖", "冲刺砖", "回旋镖花", "圈圈", "狼牙棒",
    "蛇", "机动砖", "乌卡", "陡坡", "缓坡", "卷轴相机", "中间旗", "跷跷板", "红币",
    "透明水管", "斜坡传送带", "钥匙", "蚂蚁兵", "传送箱", "小库巴", "开关", "虚线砖", "水面标记",
    "鼹鼠", "鱼骨", "太阳/月亮", "摇摆吊臂", "树", "长长吞食花", "闪烁砖", "声音", "尖刺砖",
    "机械库巴", "木箱", "蘑菇跳跳床", "尖刺河豚", "奇诺比珂", "超级锤子", "斗斗", "冰锥", "！砖", "雷米", "莫顿",
    "拉里", "温缇", "伊吉", "罗伊", "洛德威格", "炮台箱", "螺旋桨箱", "纸糊栗宝宝", "纸糊炮弹刺客", "红色POW箱", "开关跳跳床"}
    Public PT As String
    Public TileLoc(150, 2) As Point
    Public PipeLoc(3, 7) As Point
    Public GrdLoc(256) As Point
    Public Badges() As String = {"", "金缎带", "银缎带", "铜缎带", "金牌", "银牌", "铜牌"}
    Public BadgesType() As String = {"工匠点数", "耐力挑战(简单)", "耐力挑战(普通)", "耐力挑战(困难)", "耐力挑战(极难)", "多人对战", "过关关卡数", "最先通过关卡数", "最短时间关卡数", "工匠点数(周)"}
    Public GameVerStr() As String = {"1.0.0", "1.0.1", "1.1.0", "2.0.0", "3.0.0", "3.0.1"}
    Public DiffStr() As String = {"简单", "普通", "困难", "极难"}
    Public TagStr() As String = {" ", "标准", "解谜", "跑酷", "卷轴", "自动", "短图", "对战", "主题", "音乐", "艺术", "技巧", "射击", "BOSS", "单打", "林克"}
    Public CCStr() As String = {}
    Public TrackImg(15) As Bitmap
    Public Tile As Image, TileW As Integer
    Public Tile3DW(3, 3) As Image
    Public GG As Graphics, GB As Bitmap

    Public Function GetTile(x As Integer, y As Integer, w As Integer, h As Integer, Optional LX As Integer = 0, Optional LY As Integer = 0) As Image
        GB = New Bitmap(TileW * w, TileW * h)
        GG = Graphics.FromImage(GB)
        If LH.GameStyle = 22323 Then
            GG.DrawImage(Tile3DW(LX Mod 4, LY Mod 4), New Rectangle(0, 0, TileW * w, TileW * h), New Rectangle(TileW * x, TileW * y, TileW * w, TileW * h), GraphicsUnit.Pixel)
        Else
            GG.DrawImage(Tile, New Rectangle(0, 0, TileW * w, TileW * h), New Rectangle(TileW * x, TileW * y, TileW * w, TileW * h), GraphicsUnit.Pixel)
        End If
        GetTile = GB
    End Function
    Public Sub InitTrackImg()
        Dim B As Bitmap, G As Graphics
        For i As Integer = 0 To 15
            If i < 8 Then
                '3*3
                B = New Bitmap(192, 192)
            Else
                '5*5
                B = New Bitmap(320, 320)
            End If
            G = Graphics.FromImage(B)
            Select Case i
                Case 0 '横128 96 129
                    G.DrawImage(GetTile(128 Mod 16, 128 \ 16, 1, 1), 0, 64)
                    G.DrawImage(GetTile(96 Mod 16, 96 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(129 Mod 16, 129 \ 16, 1, 1), 128, 64)
                Case 1 '竖130 97 131
                    G.DrawImage(GetTile(130 Mod 16, 130 \ 16, 1, 1), 64, 0)
                    G.DrawImage(GetTile(97 Mod 16, 97 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(131 Mod 16, 131 \ 16, 1, 1), 64, 128)
                Case 2 '右斜132 98 133
                    G.DrawImage(GetTile(132 Mod 16, 132 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(98 Mod 16, 98 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(133 Mod 16, 133 \ 16, 1, 1), 128, 128)
                Case 3 '左斜135 99 134
                    G.DrawImage(GetTile(135 Mod 16, 135 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(99 Mod 16, 99 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(134 Mod 16, 134 \ 16, 1, 1), 0, 128)
                Case 4 '左下弯130 172 173 174 175 129
                    G.DrawImage(GetTile(130 Mod 16, 130 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(172 Mod 16, 172 \ 16, 1, 1), 0, 64)
                    G.DrawImage(GetTile(173 Mod 16, 173 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(174 Mod 16, 174 \ 16, 1, 1), 0, 128)
                    G.DrawImage(GetTile(175 Mod 16, 175 \ 16, 1, 1), 64, 128)
                    G.DrawImage(GetTile(129 Mod 16, 129 \ 16, 1, 1), 128, 128)
                Case 5 '右上弯128 176 177 178 179 131
                    G.DrawImage(GetTile(128 Mod 16, 128 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(176 Mod 16, 176 \ 16, 1, 1), 64, 0)
                    G.DrawImage(GetTile(177 Mod 16, 177 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(178 Mod 16, 178 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(179 Mod 16, 179 \ 16, 1, 1), 128, 64)
                    G.DrawImage(GetTile(131 Mod 16, 131 \ 16, 1, 1), 128, 128)
                Case 6 '左上弯129 169 168 171 170 131
                    G.DrawImage(GetTile(129 Mod 16, 129 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(169 Mod 16, 169 \ 16, 1, 1), 64, 0)
                    G.DrawImage(GetTile(168 Mod 16, 168 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(171 Mod 16, 171 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(170 Mod 16, 170 \ 16, 1, 1), 0, 64)
                    G.DrawImage(GetTile(131 Mod 16, 131 \ 16, 1, 1), 0, 128)
                Case 7 '右下弯130 181 180 183 182 128
                    G.DrawImage(GetTile(130 Mod 16, 130 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(181 Mod 16, 181 \ 16, 1, 1), 128, 64)
                    G.DrawImage(GetTile(180 Mod 16, 180 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(183 Mod 16, 183 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(182 Mod 16, 182 \ 16, 1, 1), 64, 128)
                    G.DrawImage(GetTile(128 Mod 16, 128 \ 16, 1, 1), 0, 128)
                Case 8 'Y左下135 517 / 128 96 560 / 98 133
                    G.DrawImage(GetTile(135 Mod 16, 135 \ 16, 1, 1), 256, 0)
                    G.DrawImage(GetTile(517 Mod 16, 517 \ 16, 1, 1), 192, 64)
                    G.DrawImage(GetTile(128 Mod 16, 128 \ 16, 1, 1), 0, 128)
                    G.DrawImage(GetTile(96 Mod 16, 96 \ 16, 1, 1), 64, 128)
                    G.DrawImage(GetTile(560 Mod 16, 560 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(98 Mod 16, 98 \ 16, 1, 1), 192, 192)
                    G.DrawImage(GetTile(133 Mod 16, 133 \ 16, 1, 1), 256, 256)
                Case 9 'Y右下132 516 / 564 96 129 / 99 134
                    G.DrawImage(GetTile(132 Mod 16, 132 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(516 Mod 16, 516 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(564 Mod 16, 564 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(96 Mod 16, 96 \ 16, 1, 1), 192, 128)
                    G.DrawImage(GetTile(129 Mod 16, 129 \ 16, 1, 1), 256, 128)
                    G.DrawImage(GetTile(99 Mod 16, 99 \ 16, 1, 1), 64, 192)
                    G.DrawImage(GetTile(134 Mod 16, 134 \ 16, 1, 1), 0, 256)
                Case 10 'Y下左132 98 / 135 517 / 572 97 131
                    G.DrawImage(GetTile(132 Mod 16, 132 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(98 Mod 16, 98 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(135 Mod 16, 135 \ 16, 1, 1), 256, 0)
                    G.DrawImage(GetTile(517 Mod 16, 517 \ 16, 1, 1), 192, 64)
                    G.DrawImage(GetTile(572 Mod 16, 572 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(97 Mod 16, 97 \ 16, 1, 1), 128, 192)
                    G.DrawImage(GetTile(131 Mod 16, 131 \ 16, 1, 1), 128, 256)
                Case 11 'Y上左130 97 568 / 99 134 / 516 133
                    G.DrawImage(GetTile(130 Mod 16, 130 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(97 Mod 16, 97 \ 16, 1, 1), 128, 64)
                    G.DrawImage(GetTile(568 Mod 16, 568 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(99 Mod 16, 99 \ 16, 1, 1), 64, 192)
                    G.DrawImage(GetTile(134 Mod 16, 134 \ 16, 1, 1), 0, 256)
                    G.DrawImage(GetTile(516 Mod 16, 516 \ 16, 1, 1), 192, 192)
                    G.DrawImage(GetTile(133 Mod 16, 133 \ 16, 1, 1), 256, 256)
                Case 12 'Y左上135 99 / 128 96 562 / 516 133
                    G.DrawImage(GetTile(135 Mod 16, 135 \ 16, 1, 1), 256, 0)
                    G.DrawImage(GetTile(99 Mod 16, 99 \ 16, 1, 1), 192, 64)
                    G.DrawImage(GetTile(128 Mod 16, 128 \ 16, 1, 1), 0, 128)
                    G.DrawImage(GetTile(96 Mod 16, 96 \ 16, 1, 1), 64, 128)
                    G.DrawImage(GetTile(562 Mod 16, 562 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(516 Mod 16, 516 \ 16, 1, 1), 192, 192)
                    G.DrawImage(GetTile(133 Mod 16, 133 \ 16, 1, 1), 256, 256)
                Case 13 'Y右上132 98 / 566 96 129 / 517 134
                    G.DrawImage(GetTile(132 Mod 16, 132 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(98 Mod 16, 98 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(566 Mod 16, 566 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(96 Mod 16, 96 \ 16, 1, 1), 192, 128)
                    G.DrawImage(GetTile(129 Mod 16, 129 \ 16, 1, 1), 256, 128)
                    G.DrawImage(GetTile(517 Mod 16, 517 \ 16, 1, 1), 64, 192)
                    G.DrawImage(GetTile(134 Mod 16, 134 \ 16, 1, 1), 0, 256)
                Case 14 'Y下右132 516 / 135 99 / 574 97 131
                    G.DrawImage(GetTile(132 Mod 16, 132 \ 16, 1, 1), 0, 0)
                    G.DrawImage(GetTile(516 Mod 16, 516 \ 16, 1, 1), 64, 64)
                    G.DrawImage(GetTile(135 Mod 16, 135 \ 16, 1, 1), 256, 0)
                    G.DrawImage(GetTile(99 Mod 16, 99 \ 16, 1, 1), 192, 64)
                    G.DrawImage(GetTile(574 Mod 16, 574 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(97 Mod 16, 97 \ 16, 1, 1), 128, 192)
                    G.DrawImage(GetTile(131 Mod 16, 131 \ 16, 1, 1), 128, 256)
                Case 15 'Y上右130 97 570 / 517 134 / 98 133
                    G.DrawImage(GetTile(130 Mod 16, 130 \ 16, 1, 1), 128, 0)
                    G.DrawImage(GetTile(97 Mod 16, 97 \ 16, 1, 1), 128, 64)
                    G.DrawImage(GetTile(570 Mod 16, 570 \ 16, 1, 1), 128, 128)
                    G.DrawImage(GetTile(517 Mod 16, 517 \ 16, 1, 1), 64, 192)
                    G.DrawImage(GetTile(134 Mod 16, 134 \ 16, 1, 1), 0, 256)
                    G.DrawImage(GetTile(98 Mod 16, 98 \ 16, 1, 1), 192, 192)
                    G.DrawImage(GetTile(133 Mod 16, 133 \ 16, 1, 1), 256, 256)
            End Select
            TrackImg(i) = B.Clone
        Next
    End Sub

    Public Function GetItemName(n As Integer, v As Integer) As String
        Select Case v
            Case 12621
                GetItemName = ObjEng(n)
            Case 13133
                GetItemName = ObjEng3(n)
            Case 22349
                GetItemName = ObjEngW(n)
            Case 21847
                GetItemName = ObjEngU(n)
            Case 22323
                GetItemName = ObjEngD(n)
            Case Else
                GetItemName = "?"
        End Select
    End Function
    Function R(size As Integer, offset As Integer)
        Select Case size
            Case 1
                Return LvlData(offset)
            Case 2
                Return BitConverter.ToInt16(LvlData, offset)
            Case 4
                Return BitConverter.ToInt32(LvlData, offset)
            Case 8
                Return BitConverter.ToInt64(LvlData, offset)
            Case 3 '游戏版本
                Return BitConverter.ToChar(LvlData, offset)
            Case 66, 202 '字符串
                Dim i As Long, S As String, K As Int16
                S = ""
                For i = 0 To size - 1 Step 2
                    K = R(2, offset + i)
                    If K = 0 Then Exit For
                    S += ChrW(K)
                Next
                Return S
            Case Else '其他
                Return 0
        End Select
    End Function
    Public Sub LoadLvlData(P As String)
        LvlData = File.ReadAllBytes(PT & "\decrypt_data\" & P)
        '文件头 长度512
        With LH
            .StartY = R(1, 0)
            .GoalY = R(1, 1)
            .GoalX = R(2, 2)
            .Timer = R(2, 4)
            .ClearCA = R(2, 6)
            .DateYY = R(2, 8)
            .DateMM = R(1, 10)
            .DateDD = R(1, 11)
            .DateH = R(1, 12)
            .DateM = R(1, 13)
            .AutoscrollSpd = R(1, 14)
            .ClearCC = R(1, 15)
            .ClearCRC = R(4, 16)
            .GameVer = R(4, 20)
            .MFlag = R(4, 24)
            .ClearAttempts = R(4, 28)
            .ClearTime = R(4, 32)
            .CreationID = R(4, 36)
            .UploadID = R(8, 40)
            .ClearVer = R(4, 48)
            .GameStyle = R(2, 241)
            .Name = R(66, 244)
            .Desc = R(202, 310)
        End With

        'FileOpen(1, PT & "\decrypt_data\" & P, OpenMode.Binary)
        'Offset = IIf(IO, &H200, &H2E0E0)

        Dim Offset As Integer, M As Long
        ObjLocData = New ObjStr(1, 300, 300) {}

        For N As Integer = 0 To 1
            Offset = IIf(N = 0, 512, 188640)
            '表世界512 里世界188640 长度188128
            '文件头72
            With MH(N)
                .Theme = R(1, Offset)
                .AutoscrollType = R(1, Offset + 1)
                .BorFlag = R(1, Offset + 2)
                .Ori = R(1, Offset + 3)
                .LiqEHeight = R(1, Offset + 4)
                .LiqMode = R(1, Offset + 5)
                .LiqSpd = R(1, Offset + 6)
                .LiqSHeight = R(1, Offset + 7)
                .BorR = R(4, Offset + 8)
                .BorT = R(4, Offset + 12)
                .BorL = R(4, Offset + 16)
                .BorB = R(4, Offset + 20)
                .Flag = R(4, Offset + 24)
                .ObjCount = R(4, Offset + 28)
                .SndCount = R(4, Offset + 32)
                .SnakeCount = R(4, Offset + 36)
                .ClearPipCount = R(4, Offset + 40)
                .CreeperCount = R(4, Offset + 44)
                .iBlkCount = R(4, Offset + 48)
                .TrackBlkCount = R(4, Offset + 52)
                'R(4, Offset + 56)
                .GroundCount = R(4, Offset + 60)
                .TrackCount = R(4, Offset + 64)
                .IceCount = R(4, Offset + 68)
            End With
            Debug.Print("=================")
            Debug.Print("ObjCount:" & MH(N).ObjCount.ToString)
            Debug.Print("SndCount:" & MH(N).SndCount.ToString)
            Debug.Print("SnakeCount:" & MH(N).SnakeCount.ToString)
            Debug.Print("ClearPipCount:" & MH(N).ClearPipCount.ToString)
            Debug.Print("CreeperCount:" & MH(N).CreeperCount.ToString)
            Debug.Print("iBlkCount:" & MH(N).iBlkCount.ToString)
            Debug.Print("TrackBlkCount:" & MH(N).TrackBlkCount.ToString)
            Debug.Print("GroundCount:" & MH(N).GroundCount.ToString)
            Debug.Print("TrackCount:" & MH(N).TrackCount.ToString)
            Debug.Print("IceCount:" & MH(N).IceCount.ToString)
            '单位32*2600
            For M = 0 To MH(N).ObjCount - 1
                With MapObj(N, M)
                    .X = R(4, Offset + &H48 + &H0 + M * &H20)
                    .Y = R(4, Offset + &H48 + &H4 + M * &H20)
                    .W = R(1, Offset + &H48 + &HA + M * &H20)
                    .H = R(1, Offset + &H48 + &HB + M * &H20)
                    .Flag = R(4, Offset + &H48 + &HC + M * &H20)
                    .CFlag = R(4, Offset + &H48 + &H10 + M * &H20)
                    .Ex = R(4, Offset + &H48 + &H14 + M * &H20)
                    .ID = R(2, Offset + &H48 + &H18 + M * &H20)
                    .CID = R(2, Offset + &H48 + &H1A + M * &H20)
                    .LID = R(2, Offset + &H48 + &H1C + M * &H20)
                    .SID = R(2, Offset + &H48 + &H1E + M * &H20)
                    .LinkType = 0
                End With
            Next

            '音效4*300
            For M = 0 To MH(N).SndCount - 1
                With MapSnd(N, M)
                    .X = R(1, Offset + &H14584 + &H0 + M * &H4)
                    .Y = R(1, Offset + &H14584 + &H1 + M * &H4)
                    .ID = R(1, Offset + &H14584 + &H2 + M * &H4)
                End With
            Next

            '蛇砖块964*5
            For M = 0 To MH(N).SnakeCount - 1
                With MapSnk(N, M)
                    .index = R(1, Offset + &H149F8 + &H0 + M * &H3C4)
                    .NodeCount = R(1, Offset + &H149F8 + &H1 + M * &H3C4)
                    ReDim .Node(.NodeCount - 1)
                    For i = 0 To .NodeCount - 1
                        .Node(i).index = R(1, Offset + &H149F8 + &H0 + M * &H3C4 + i * &H8)
                        .Node(i).Dir = R(1, Offset + &H149F8 + &H6 + M * &H3C4 + i * &H8)
                    Next
                End With
            Next

            '透明管292*200
            For M = 0 To MH(N).ClearPipCount - 1
                With MapCPipe(N, M)
                    .Index = R(1, Offset + &H15CCC + &H0 + M * &H124)
                    .NodeCount = R(1, Offset + &H15CCC + &H1 + M * &H124)
                    ReDim .Node(.NodeCount - 1)
                    For i = 0 To .NodeCount - 1
                        .Node(i).type = R(1, Offset + &H15CCC + &H4 + M * &H124 + i * &H8)
                        .Node(i).index = R(1, Offset + &H15CCC + &H5 + M * &H124 + i * &H8)
                        .Node(i).X = R(1, Offset + &H15CCC + &H6 + M * &H124 + i * &H8)
                        .Node(i).Y = R(1, Offset + &H15CCC + &H7 + M * &H124 + i * &H8)
                        .Node(i).W = R(1, Offset + &H15CCC + &H8 + M * &H124 + i * &H8)
                        .Node(i).H = R(1, Offset + &H15CCC + &H9 + M * &H124 + i * &H8)
                        .Node(i).Dir = R(1, Offset + &H15CCC + &HB + M * &H124 + i * &H8)
                    Next
                End With
            Next

            '长长食人花84*10
            For M = 0 To MH(N).CreeperCount - 1
                With MapCrp(N, M)
                    .index = R(1, Offset + &H240EC + &H1 + M * &H54)
                    .NodeCount = R(2, Offset + &H240EC + &H2 + M * &H54)
                    ReDim .Node(.NodeCount - 1)
                    For i = 0 To .NodeCount - 1
                        .Node(i) = R(1, Offset + &H240EC + &H4 + &H1 + M * &H54 + i * &H4)
                    Next
                End With
            Next

            '！砖44*10
            For M = 0 To MH(N).iBlkCount - 1
                With MapMoveBlk(N, M)
                    .index = R(1, Offset + &H24434 + &H1 + M * &H2C)
                    .NodeCount = R(2, Offset + &H24434 + &H2 + M * &H2C)
                    ReDim .Node(.NodeCount - 1)
                    For i = 0 To .NodeCount - 1
                        .Node(i).p0 = R(1, Offset + &H24434 + &H4 + &H0 + M * &H2C + i * &H4)
                        .Node(i).p1 = R(1, Offset + &H24434 + &H4 + &H1 + M * &H2C + i * &H4)
                        .Node(i).p2 = R(1, Offset + &H24434 + &H4 + &H2 + M * &H2C + i * &H4)
                    Next
                End With
            Next

            '机动砖44*10
            For M = 0 To MH(N).TrackBlkCount - 1
                With MapTrackBlk(N, M)
                    .index = R(1, Offset + &H245EC + &H1 + M * &H2C)
                    .NodeCount = R(2, Offset + &H245EC + &H2 + M * &H2C)
                    ReDim .Node(.NodeCount - 1)
                    For i = 0 To .NodeCount - 1
                        .Node(i).p0 = R(1, Offset + &H245EC + &H4 + &H0 + M * &H2C + i * &H4)
                        .Node(i).p1 = R(1, Offset + &H245EC + &H4 + &H1 + M * &H2C + i * &H4)
                        .Node(i).p2 = R(1, Offset + &H245EC + &H4 + &H2 + M * &H2C + i * &H4)
                    Next
                End With
            Next
            '地砖4*4000
            For M = 0 To 300
                For j = 0 To 300
                    GroundNode(N, M, j) = 0
                Next
            Next
            For M = 0 To MH(N).GroundCount - 1
                With MapGrd(N, M)
                    .X = R(1, Offset + &H247A4 + &H0 + M * &H4)
                    .Y = R(1, Offset + &H247A4 + &H1 + M * &H4)
                    .ID = R(1, Offset + &H247A4 + &H2 + M * &H4)
                    .BID = R(1, Offset + &H247A4 + &H3 + M * &H4)
                    GroundNode(N, .X + 1, .Y + 1) = 1
                End With
            Next
            If N = 0 Then
                For j = (LH.GoalX - 5) \ 10 To (LH.GoalX - 5) \ 10 + 9
                    For i = 0 To LH.GoalY - 1
                        GroundNode(0, j + 1, i + 1) = 1
                    Next
                Next
                For j = 0 To 6
                    For i = 0 To LH.StartY - 1
                        GroundNode(0, j + 1, i + 1) = 1
                    Next
                Next
            End If

            '轨道12*1500
            'TrackNode(MapHdr.BorR + 3, MapHdr.BorT + 3)
            Dim TX As Byte
            For M = 0 To MH(N).TrackCount - 1
                With MapTrk(N, M)
                    .UN = R(2, Offset + &H28624 + &H0 + M * &HC)
                    .Flag = R(1, Offset + &H28624 + &H2 + M * &HC)
                    TX = R(1, Offset + &H28624 + &H3 + M * &HC)
                    If TX = 255 Then
                        .X = 0
                    Else
                        .X = TX + 1
                    End If
                    TX = R(1, Offset + &H28624 + &H4 + M * &HC)
                    If TX = 255 Then
                        .Y = 0
                    Else
                        .Y = TX + 1
                    End If
                    .Type = R(1, Offset + &H28624 + &H5 + M * &HC)
                    .LID = R(2, Offset + &H28624 + &H6 + M * &HC)
                    .K0 = R(4, Offset + &H28624 + &H8 + M * &HC)
                    .K1 = R(4, Offset + &H28624 + &HA + M * &HC)
                    Select Case .Type
                        Case 0
                            TrackNode(N, .X, .Y + 1) += 1
                            TrackNode(N, .X + 2, .Y + 1) += 1
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K1 >> 7) Mod 2
                        Case 1
                            TrackNode(N, .X + 1, .Y + 2) += 1
                            TrackNode(N, .X + 1, .Y) += 1
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K1 >> 7) Mod 2
                        Case 2, 4, 5
                            TrackNode(N, .X, .Y + 2) += 1
                            TrackNode(N, .X + 2, .Y) += 1
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K1 >> 7) Mod 2
                        Case 3, 6, 7
                            TrackNode(N, .X + 2, .Y + 2) += 1
                            TrackNode(N, .X, .Y) += 1
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K1 >> 7) Mod 2
                        Case 8
                            'F0标记
                            .F0 = (.K1 >> 6) Mod 2
                            .F1 = (.K0 >> 7) Mod 2
                            .F2 = ((.K0 >> 15) + 1) Mod 2
                            TrackNode(N, .X, .Y + 2) += 1
                            TrackNode(N, .X + 4, .Y) += 1
                            TrackNode(N, .X + 4, .Y + 4) += 1
                        Case 9
                            .F0 = (.K1 >> 6) Mod 2
                            .F1 = (.K1 >> 1) Mod 2
                            .F2 = (.K0 >> 7) Mod 2
                            TrackNode(N, .X, .Y) += 1
                            TrackNode(N, .X, .Y + 4) += 1
                            TrackNode(N, .X + 4, .Y + 2) += 1
                        Case 10
                            .F0 = ((.K0 >> 14) + 1) Mod 2
                            .F1 = (.K1 >> 6) Mod 2
                            .F2 = (.K0 >> 7) Mod 2
                            TrackNode(N, .X, .Y) += 1
                            TrackNode(N, .X + 2, .Y + 4) += 1
                            TrackNode(N, .X + 4, .Y) += 1
                        Case 11
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K1 >> 1) Mod 2
                            .F2 = (.K1 >> 6) Mod 2
                            TrackNode(N, .X + 2, .Y) += 1
                            TrackNode(N, .X, .Y + 4) += 1
                            TrackNode(N, .X + 4, .Y + 4) += 1
                        Case 12
                            .F0 = (.K1 >> 11) Mod 2
                            .F1 = (.K0 >> 7) Mod 2
                            .F2 = (.K0 >> 12) Mod 2
                            TrackNode(N, .X, .Y + 2) += 1
                            TrackNode(N, .X + 4, .Y) += 1
                            TrackNode(N, .X + 4, .Y + 4) += 1
                        Case 13
                            .F0 = (.K1 >> 11) Mod 2
                            .F1 = (.K0 >> 12) Mod 2
                            .F2 = (.K0 >> 7) Mod 2
                            TrackNode(N, .X, .Y) += 1
                            TrackNode(N, .X, .Y + 4) += 1
                            TrackNode(N, .X + 4, .Y + 2) += 1
                        Case 14
                            .F0 = (.K0 >> 12) Mod 2
                            .F1 = (.K1 >> 11) Mod 2
                            .F2 = (.K0 >> 7) Mod 2
                            TrackNode(N, .X, .Y) += 1
                            TrackNode(N, .X + 4, .Y) += 1
                            TrackNode(N, .X + 2, .Y + 4) += 1
                        Case 15
                            .F0 = (.K0 >> 7) Mod 2
                            .F1 = (.K0 >> 12) Mod 2
                            .F2 = (.K1 >> 11) Mod 2
                            TrackNode(N, .X + 2, .Y) += 1
                            TrackNode(N, .X, .Y + 4) += 1
                            TrackNode(N, .X + 4, .Y + 4) += 1
                    End Select
                End With
            Next

            '冰锥4*300
            For M = 0 To MH(N).IceCount - 1
                With MapIce(N, M)
                    .X = R(1, Offset + &H2CC74 + &H0 + M * &H4)
                    .Y = R(1, Offset + &H2CC74 + &H1 + M * &H4)
                    .ID = R(1, Offset + &H2CC74 + &H2 + M * &H4)
                End With
            Next

            '保留3516
        Next
    End Sub

    Public Structure ObjStr
        Dim Obj As String
        Dim Flag As String
        Dim State As String
        Dim SubObj As String
        Dim SubFlag As String
        Dim SubState As String
    End Structure
    Public ObjLocData(,,) As ObjStr
    Public Function GetItemImg(Obj As ObjStr, ByRef W As Integer, ByRef H As Integer) As Image
        Dim S0(), S1(), S2(), S3(), S4(), S5() As String
        If IsNothing(Obj.Obj) Then
            W = 0
            H = 0
            GetItemImg = Nothing
        Else
            S0 = Obj.Obj.Split(",")
            S1 = Obj.Flag.Split(",")
            S2 = Obj.State.Split(",")
            S3 = Obj.SubObj.Split(",")
            S4 = Obj.SubFlag.Split(",")
            S5 = Obj.SubState.Split(",")
            H = S0.GetUpperBound(0) * (64 + 6) - 6
            W = 64 * 2 + 32
            Dim BB As Bitmap = New Bitmap(W, H)
            Dim GG As Graphics = Graphics.FromImage(BB)
            'GG.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            Dim I, J As Integer
            For I = 0 To S0.GetUpperBound(0) - 1
                If S0(I).Length > 0 Then
                    GG.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\DLY\" & S0(I) & S2(I) & ".PNG"), 0, I * 70, 64, 64)
                    For J = 1 To S1(I).Length
                        GG.DrawImage(SetOpacity(Image.FromFile(PT & "\IMG\CMN\F0.PNG"), 1), J * 24 - 20, I * 70 + 70 - 32, 24, 24)
                        GG.DrawImage(SetOpacity(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\J" & Strings.Mid(S1(I), J, 1) & ".PNG"), 1), J * 24 - 20, I * 70 + 70 - 32, 24, 24)
                    Next
                    If S3(I).Length > 0 Then
                        GG.DrawImage(Image.FromFile(PT & "\IMG\CMN\G0.PNG"), 64, I * 70 + 16, 32, 32)
                        GG.DrawImage(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\DLY\" & S3(I) & S5(I) & ".PNG"), 88, I * 70, 64, 64)
                        For J = 1 To S4(I).Length
                            GG.DrawImage(SetOpacity(Image.FromFile(PT & "\IMG\CMN\F0.PNG"), 1), 88 - 20 + J * 24, I * 70 + 70 - 32, 24, 24)
                            GG.DrawImage(SetOpacity(Image.FromFile(PT & "\IMG\" & LH.GameStyle.ToString & "\J" & Strings.Mid(S4(I), J, 1) & ".PNG"), 1), 88 - 20 + J * 24, I * 70 + 70 - 32, 24, 24)

                        Next
                    End If
                End If
            Next
            GetItemImg = BB
        End If
    End Function
    Public Function Magnifier(srcB As Bitmap, multiple As Integer) As Bitmap
        If multiple <= 0 Then
            Return srcB
        End If
        Dim B As Bitmap = New Bitmap(srcB.Size.Width * multiple, srcB.Size.Height * multiple)
        Dim srcData As BitmapData = srcB.LockBits(New Rectangle(New Point(0, 0), srcB.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
        Dim BData As BitmapData = B.LockBits(New Rectangle(New Point(0, 0), B.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
        Dim srcPtr As IntPtr = srcData.Scan0
        Dim BPtr As IntPtr = BData.Scan0
        For y As Integer = 0 To srcData.Height - 1
            For x As Integer = 0 To srcData.Width - 1
                For i As Integer = 0 To multiple - 1
                    For j As Integer = 0 To multiple - 1
                        Marshal.WriteInt32(BPtr, (x * multiple + i + (y * multiple + j) * BData.Width) * 4, Marshal.ReadInt32(srcPtr, (x + y * srcData.Width) * 4))
                    Next
                Next
            Next
        Next
        srcB.UnlockBits(srcData)
        B.UnlockBits(BData)
        Return B
    End Function

    Public Function SetOpacity(ByVal B As Bitmap, ByVal D As Double) As Bitmap
        Try
            Dim bmpDATA As New Imaging.BitmapData
            Dim tmpBMP = New Bitmap(B)
            Dim Rct As Rectangle = New Rectangle(0, 0, B.Width, B.Height)
            bmpDATA = tmpBMP.LockBits(Rct, Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
            Dim BTS(bmpDATA.Stride * bmpDATA.Height) As Byte
            Runtime.InteropServices.Marshal.Copy(bmpDATA.Scan0, BTS, 0, BTS.Length - 1)
            Dim T As Double = 0
            For I As Integer = 0 To BTS.Length - 4 Step 4
                T = BTS(I + 3)
                T = T * D
                BTS(I + 3) = T
            Next
            Runtime.InteropServices.Marshal.Copy(BTS, 0, bmpDATA.Scan0, BTS.Length - 1)
            tmpBMP.UnlockBits(bmpDATA)
            Return tmpBMP
        Catch
            Return Nothing
        End Try
    End Function

    Public GrdTbl_Tile() As Integer = {
        246, 254, 417, 483, 253, 237, 416, 480, 252, 239,
        443, 487, 240, 235, 438, 510, 401, 427, 423, 429,
        467, 471, 463, 473, 251, 241, 439, 511, 238, 234,
        442, 486, 236, 233, 445, 489, 232, 223, 444, 488,
        400, 422, 426, 428, 464, 462, 470, 472, 249, 231,
        421, 457, 230, 219, 420, 454, 481, 506, 490, 492,
        482, 491, 507, 493, 247, 227, 440, 225, 221, 446,
        424, 430, 250, 215, 508, 494, 243, 229, 437, 505,
        228, 220, 436, 502, 210, 503, 504, 244, 217, 211,
        484, 465, 458, 474, 476, 455, 460, 478, 468, 466,
        475, 459, 477, 456, 245, 226, 441, 224, 222, 447,
        425, 431, 248, 216, 509, 495, 213, 214, 242, 218,
        209, 485, 212, 208, 461, 479, 469}
    Public GrdTbl_Code() As Integer = {
        0, 1, 2, 3, 4, 5, 8, 12, 16, 17,
        18, 19, 20, 21, 24, 28, 32, 33, 36, 37,
        48, 49, 52, 53, 64, 65, 66, 67, 68, 69,
        72, 76, 80, 81, 82, 83, 84, 85, 88, 92,
        128, 129, 132, 133, 192, 193, 196, 197, 256, 272,
        288, 304, 320, 336, 384, 448, 512, 528, 576, 592,
        768, 784, 832, 848, 1024, 1028, 1032, 1088, 1092, 1096,
        1152, 1156, 1280, 1344, 1536, 1600, 4096, 4097, 4098, 4099,
        4100, 4101, 4104, 4108, 4352, 4608, 4864, 5120, 5124, 5376,
        5632, 8192, 8193, 8196, 8197, 8448, 9216, 9220, 9472, 12288,
        12289, 12292, 12293, 12544, 16384, 16385, 16386, 16400, 16401, 16402,
        16416, 16417, 16640, 16656, 17152, 17168, 17408, 17664, 20480, 20481,
        20736, 21248, 21504, 21760, 28672, 28673, 28928}
End Module

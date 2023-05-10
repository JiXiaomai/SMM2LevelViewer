Imports System.IO

Module ModuleDecryptor

    Const dataSize As Integer = &H5C000 - &H40
    Const endOffset As Integer = &H5C000 - &H30

    Function Decrypt(course() As Byte) As Byte()
        '.BCD->.DAT
        Dim endBytes As IEnumerable(Of Byte) = course.Skip(endOffset)
        Dim randBytes() As Byte = endBytes.Skip(&H10).Take(&H10).ToArray()
        Dim encryData() As Byte = course.Skip(&H10).Take(dataSize).ToArray()
        Dim keyBytes() As Byte = GetRandKey(randBytes)
        Dim ivBytes() As Byte = endBytes.Take(&H10).ToArray()
        Return AesDecrypt(encryData, keyBytes, ivBytes)
    End Function
    Function Encrypt(raw() As Byte) As Byte()
        '.DAT->.BCD
        Dim Rand As Random = New Random(Environment.TickCount)
        Dim iv(15) As Byte
        Dim stateSeed(15) As Byte
        Rand.NextBytes(iv)
        Rand.NextBytes(stateSeed)

        Dim cmacKey() As Byte
        Dim keyBytes() As Byte = GetRandKey(stateSeed, cmacKey)
        Dim encrypted() As Byte = AesEncrypt(raw, keyBytes, iv, AESMODE.CBC, AESPadding.NONE)

        Dim crc32 As UInteger = GetCRC32(raw)
        Dim cmac() As Byte = AesCMAC(cmacKey, raw)


        Dim R(), T0(), T1(), T2(), T3() As Byte
        R = BitConverter.GetBytes(CUInt(&H1))
        T0 = BitConverter.GetBytes(CUShort(&H10))
        T1 = BitConverter.GetBytes(CUShort(&H0))
        T2 = BitConverter.GetBytes(crc32)
        T3 = System.Text.Encoding.Default.GetBytes(course_MAGIC)

        Dim Stm As New MemoryStream
        Stm.Write(R, 0, R.Length)
        Stm.Write(T0, 0, T0.Length)
        Stm.Write(T1, 0, T1.Length)
        Stm.Write(T2, 0, T2.Length)
        Stm.Write(T3, 0, T3.Length)

        Stm.Write(encrypted, 0, encrypted.Length)
        Stm.Write(iv, 0, iv.Length)
        Stm.Write(stateSeed, 0, stateSeed.Length)
        Stm.Write(cmac, 0, cmac.Length)

        Return Stm.ToArray
    End Function

    Function GetCRC32(b() As Byte) As UInteger
        Dim iCount As Integer = b.Length
        Dim crc As UInteger = &HFFFFFFFFUI
        For i As Integer = 0 To iCount - 1
            crc = ((crc >> 8) And &HFFFFFFUI) Xor crcTable((crc Xor b(i)) And &HFFUI)
        Next
        Return crc Xor &HFFFFFFFFUI
    End Function
    Function AesCMAC(key() As Byte, data() As Byte) As Byte()
        Dim N0(15), N1(15), N2(15) As Byte
        Dim L() As Byte = AesEncrypt(N0, key, N1)

        Dim FirstSubkey() As Byte = Rol(L)
        If (L(0) And &H80) = &H80 Then FirstSubkey(15) = FirstSubkey(15) Xor &H87
        Dim SecondSubkey() As Byte = Rol(FirstSubkey)
        If (FirstSubkey(0) And &H80) = &H80 Then SecondSubkey(15) = SecondSubkey(15) Xor &H87

        If data.Length <> 0 And data.Length Mod 16 = 0 Then
            For j As Integer = 0 To FirstSubkey.Length - 1
                data(data.Length - 16 + j) = data(data.Length - 16 + j) Xor FirstSubkey(j)
            Next
        Else
            Dim padding(16 - data.Length Mod 16) As Byte
            padding(0) = &H80
            data = data.Concat(padding.AsEnumerable()).ToArray
            For j As Integer = 0 To FirstSubkey.Length - 1
                data(data.Length - 16 + j) = data(data.Length - 16 + j) Xor FirstSubkey(j)
            Next
        End If

        Dim encResult() As Byte = AesEncrypt(data, key, N2)
        Dim HashValue(15) As Byte
        Array.Copy(encResult, encResult.Length - HashValue.Length, HashValue, 0, HashValue.Length)
        Return HashValue
    End Function
    Function Rol(b() As Byte) As Byte()
        Dim r(b.Length - 1) As Byte
        Dim carry As Byte = 0
        For i As Integer = b.Length - 1 To 0 Step -1
            Dim u As UShort = CUShort(b(i) << 1)
            r(i) = CByte((u And &HFF) + carry)
            carry = CByte((u And &HFF00) >> 8)
        Next
        Return r
    End Function
    Function GetRandKey(ByVal data As Byte(), ByRef cmacKey As Byte()) As Byte()
        Dim randState = generateState(data)
        Dim keyState = generateKeyState(randState)
        Dim cmacKeyState = generateKeyState(randState)
        cmacKey = generateKey(cmacKeyState)
        Return generateKey(keyState)
    End Function
    Function generateKey(ByVal keyState As UInteger()) As Byte()
        Return BitConverter.GetBytes(keyState(0)).Concat(BitConverter.GetBytes(keyState(1))).Concat(BitConverter.GetBytes(keyState(2))).Concat(BitConverter.GetBytes(keyState(3))).ToArray()
    End Function
    Function generateState(ByVal data As Byte()) As UInteger()
        Dim in1 = BitConverter.ToUInt32(data, 0)
        Dim in2 = BitConverter.ToUInt32(data, 4)
        Dim in3 = BitConverter.ToUInt32(data, 8)
        Dim in4 = BitConverter.ToUInt32(data, 12)
        Dim cond = (in1 Or in2 Or in3 Or in4) <> 0
        Return New UInteger() {If(cond, in1, 1), If(cond, in2, &H6C078967), If(cond, in3, &H714ACB41), If(cond, in4, &H48077044)}
    End Function
    Function generateRand(ByRef rand_state() As UInteger) As UInteger
        Dim n As UInteger = rand_state(0) Xor rand_state(0) << 11
        rand_state(0) = rand_state(1)
        n = n Xor (n >> 8 Xor rand_state(3) Xor rand_state(3) >> 19)
        rand_state(1) = rand_state(2)
        rand_state(2) = rand_state(3)
        rand_state(3) = n
        Return n
    End Function
    Function generateKeyState(ByRef rand() As UInteger) As UInteger()
        Dim outKey() As UInteger = New UInteger(3) {0, 0, 0, 0}
        For i As Integer = 0 To STATE_SIZE - 1
            For j As Integer = 0 To NUM_ROUNDS - 1
                outKey(i) <<= 8
                Dim idx1 As UInteger = generateRand(rand) >> 26
                Dim idx2 As UInteger = generateRand(rand) >> 27
                Dim idx3 As UInteger = idx2 And 24
                Dim idx4 As UInteger = Course_key_table(idx1) >> CType(idx3, Integer)
                outKey(i) = outKey(i) Or idx4 And &HFF
            Next
        Next
        Return outKey
    End Function

    Const STATE_SIZE As Integer = 4
    Const NUM_ROUNDS As Integer = 4
    Const course_MAGIC As String = "SCDL"
    ReadOnly Course_key_table() As UInteger =
    {
        &H7AB1C9D2UI, &HCA750936UI, &H3003E59CUI, &HF261014BUI,
        &H2E25160AUI, &HED614811UI, &HF1AC6240UI, &HD59272CDUI,
        &HF38549BFUI, &H6CF5B327UI, &HDA4DB82AUI, &H820C435AUI,
        &HC95609BAUI, &H19BE08B0UI, &H738E2B81UI, &HED3C349AUI,
        &H45275D1UI, &HE0A73635UI, &H1DEBF4DAUI, &H9924B0DEUI,
        &H6A1FC367UI, &H71970467UI, &HFC55ABEBUI, &H368D7489UI,
        &HCC97D1DUI, &H17CC441EUI, &H3528D152UI, &HD0129B53UI,
        &HE12A69E9UI, &H13D1BDB7UI, &H32EAA9EDUI, &H42F41D1BUI,
        &HAEA5F51FUI, &H42C5D23CUI, &H7CC742EDUI, &H723BA5F9UI,
        &HDE5B99E3UI, &H2C0055A4UI, &HC38807B4UI, &H4C099B61UI,
        &HC4E4568EUI, &H8C29C901UI, &HE13B34ACUI, &HE7C3F212UI,
        &HB67EF941UI, &H8038965UI, &H8AFD1E6AUI, &H8E5341A3UI,
        &HA4C61107UI, &HFBAF1418UI, &H9B05EF64UI, &H3C91734EUI,
        &H82EC6646UI, &HFB19F33EUI, &H3BDE6FE2UI, &H17A84CCAUI,
        &HCCDF0CE9UI, &H50E4135CUI, &HFF2658B2UI, &H3780F156UI,
        &H7D8F5D68UI, &H517CBED1UI, &H1FCDDF0DUI, &H77A58C94UI
    }
    ReadOnly ENL_key_table() As UInteger =
    {
        &HB301CA48UI, &H5E758911UI, &HC2B349E2UI, &HF9942930UI,
        &H447AEFC0UI, &HB6B5DB5FUI, &HEE116832UI, &HB6940169UI,
        &H2503FC94UI, &H3D74B448UI, &H58411B2CUI, &H4EC8C604UI,
        &H74157415UI, &HEC5B582BUI, &HBC93A6F7UI, &HB463AF87UI,
        &H6B09D0C2UI, &H5DA54788UI, &H7C20F6D5UI, &HD5967141UI,
        &HF03C24F1UI, &H87D2A479UI, &HFC3F7C08UI, &H9A4506B7UI,
        &H8B4FA2A2UI, &H99AC2EDEUI, &H9E74DEDFUI, &H2CB60318UI,
        &HDA1AEE9EUI, &H2238F1DDUI, &H1A825163UI, &H86B03FE8UI,
        &H8BD35FBEUI, &H6E80E100UI, &H6681ACFAUI, &H61C990BDUI,
        &H70F61D95UI, &H19177A6AUI, &H729AE3CEUI, &H5FFBD958UI,
        &H9F217D87UI, &H3D478023UI, &H986690D6UI, &H19D6AB9BUI,
        &H8D8F2063UI, &H8CC8EF69UI, &H20843E06UI, &H8CA2C3FEUI,
        &H78DA6631UI, &HB3A27DE4UI, &HB2D71198UI, &H28F0890FUI,
        &H83B089CEUI, &H235D8901UI, &H290C0723UI, &H85184BFCUI,
        &H82E15C68UI, &H4D3BD8B4UI, &H447FB2FUI, &H434717F0UI,
        &HCBCD01ECUI, &H58A09E59UI, &H630588E1UI, &H1886EBE6UI
    }
    ReadOnly crcTable() As UInteger =
    {
        &H0UI, &H77073096UI, &HEE0E612CUI, &H990951BAUI, &H76DC419UI, &H706AF48FUI, &HE963A535UI, &H9E6495A3UI,
        &HEDB8832UI, &H79DCB8A4UI, &HE0D5E91EUI, &H97D2D988UI, &H9B64C2BUI, &H7EB17CBDUI, &HE7B82D07UI, &H90BF1D91UI,
        &H1DB71064UI, &H6AB020F2UI, &HF3B97148UI, &H84BE41DEUI, &H1ADAD47DUI, &H6DDDE4EBUI, &HF4D4B551UI, &H83D385C7UI,
        &H136C9856UI, &H646BA8C0UI, &HFD62F97AUI, &H8A65C9ECUI, &H14015C4FUI, &H63066CD9UI, &HFA0F3D63UI, &H8D080DF5UI,
        &H3B6E20C8UI, &H4C69105EUI, &HD56041E4UI, &HA2677172UI, &H3C03E4D1UI, &H4B04D447UI, &HD20D85FDUI, &HA50AB56BUI,
        &H35B5A8FAUI, &H42B2986CUI, &HDBBBC9D6UI, &HACBCF940UI, &H32D86CE3UI, &H45DF5C75UI, &HDCD60DCFUI, &HABD13D59UI,
        &H26D930ACUI, &H51DE003AUI, &HC8D75180UI, &HBFD06116UI, &H21B4F4B5UI, &H56B3C423UI, &HCFBA9599UI, &HB8BDA50FUI,
        &H2802B89EUI, &H5F058808UI, &HC60CD9B2UI, &HB10BE924UI, &H2F6F7C87UI, &H58684C11UI, &HC1611DABUI, &HB6662D3DUI,
        &H76DC4190UI, &H1DB7106UI, &H98D220BCUI, &HEFD5102AUI, &H71B18589UI, &H6B6B51FUI, &H9FBFE4A5UI, &HE8B8D433UI,
        &H7807C9A2UI, &HF00F934UI, &H9609A88EUI, &HE10E9818UI, &H7F6A0DBBUI, &H86D3D2DUI, &H91646C97UI, &HE6635C01UI,
        &H6B6B51F4UI, &H1C6C6162UI, &H856530D8UI, &HF262004EUI, &H6C0695EDUI, &H1B01A57BUI, &H8208F4C1UI, &HF50FC457UI,
        &H65B0D9C6UI, &H12B7E950UI, &H8BBEB8EAUI, &HFCB9887CUI, &H62DD1DDFUI, &H15DA2D49UI, &H8CD37CF3UI, &HFBD44C65UI,
        &H4DB26158UI, &H3AB551CEUI, &HA3BC0074UI, &HD4BB30E2UI, &H4ADFA541UI, &H3DD895D7UI, &HA4D1C46DUI, &HD3D6F4FBUI,
        &H4369E96AUI, &H346ED9FCUI, &HAD678846UI, &HDA60B8D0UI, &H44042D73UI, &H33031DE5UI, &HAA0A4C5FUI, &HDD0D7CC9UI,
        &H5005713CUI, &H270241AAUI, &HBE0B1010UI, &HC90C2086UI, &H5768B525UI, &H206F85B3UI, &HB966D409UI, &HCE61E49FUI,
        &H5EDEF90EUI, &H29D9C998UI, &HB0D09822UI, &HC7D7A8B4UI, &H59B33D17UI, &H2EB40D81UI, &HB7BD5C3BUI, &HC0BA6CADUI,
        &HEDB88320UI, &H9ABFB3B6UI, &H3B6E20CUI, &H74B1D29AUI, &HEAD54739UI, &H9DD277AFUI, &H4DB2615UI, &H73DC1683UI,
        &HE3630B12UI, &H94643B84UI, &HD6D6A3EUI, &H7A6A5AA8UI, &HE40ECF0BUI, &H9309FF9DUI, &HA00AE27UI, &H7D079EB1UI,
        &HF00F9344UI, &H8708A3D2UI, &H1E01F268UI, &H6906C2FEUI, &HF762575DUI, &H806567CBUI, &H196C3671UI, &H6E6B06E7UI,
        &HFED41B76UI, &H89D32BE0UI, &H10DA7A5AUI, &H67DD4ACCUI, &HF9B9DF6FUI, &H8EBEEFF9UI, &H17B7BE43UI, &H60B08ED5UI,
        &HD6D6A3E8UI, &HA1D1937EUI, &H38D8C2C4UI, &H4FDFF252UI, &HD1BB67F1UI, &HA6BC5767UI, &H3FB506DDUI, &H48B2364BUI,
        &HD80D2BDAUI, &HAF0A1B4CUI, &H36034AF6UI, &H41047A60UI, &HDF60EFC3UI, &HA867DF55UI, &H316E8EEFUI, &H4669BE79UI,
        &HCB61B38CUI, &HBC66831AUI, &H256FD2A0UI, &H5268E236UI, &HCC0C7795UI, &HBB0B4703UI, &H220216B9UI, &H5505262FUI,
        &HC5BA3BBEUI, &HB2BD0B28UI, &H2BB45A92UI, &H5CB36A04UI, &HC2D7FFA7UI, &HB5D0CF31UI, &H2CD99E8BUI, &H5BDEAE1DUI,
        &H9B64C2B0UI, &HEC63F226UI, &H756AA39CUI, &H26D930AUI, &H9C0906A9UI, &HEB0E363FUI, &H72076785UI, &H5005713UI,
        &H95BF4A82UI, &HE2B87A14UI, &H7BB12BAEUI, &HCB61B38UI, &H92D28E9BUI, &HE5D5BE0DUI, &H7CDCEFB7UI, &HBDBDF21UI,
        &H86D3D2D4UI, &HF1D4E242UI, &H68DDB3F8UI, &H1FDA836EUI, &H81BE16CDUI, &HF6B9265BUI, &H6FB077E1UI, &H18B74777UI,
        &H88085AE6UI, &HFF0F6A70UI, &H66063BCAUI, &H11010B5CUI, &H8F659EFFUI, &HF862AE69UI, &H616BFFD3UI, &H166CCF45UI,
        &HA00AE278UI, &HD70DD2EEUI, &H4E048354UI, &H3903B3C2UI, &HA7672661UI, &HD06016F7UI, &H4969474DUI, &H3E6E77DBUI,
        &HAED16A4AUI, &HD9D65ADCUI, &H40DF0B66UI, &H37D83BF0UI, &HA9BCAE53UI, &HDEBB9EC5UI, &H47B2CF7FUI, &H30B5FFE9UI,
        &HBDBDF21CUI, &HCABAC28AUI, &H53B39330UI, &H24B4A3A6UI, &HBAD03605UI, &HCDD70693UI, &H54DE5729UI, &H23D967BFUI,
        &HB3667A2EUI, &HC4614AB8UI, &H5D681B02UI, &H2A6F2B94UI, &HB40BBE37UI, &HC30C8EA1UI, &H5A05DF1BUI, &H2D02EF8DUI
    }

    Function Create_key_part() As Byte
        Dim R As New Random
        Dim I, V, S, B As Byte
        V = 0
        For I = 0 To 3
            I = R.Next(UBound(ENL_key_table))
            S = R.Next(4) * 8
            B = (ENL_key_table(I) >> S) And &HFF
            V = (V << 8) Or B
        Next
        Return V
    End Function
    Function Create_key(size As Byte) As String
        Dim i, v As Byte
        Dim key As String = ""
        For i = 0 To size \ 4
            v = Create_key_part()
            key += Hex(v)
        Next
        Return key
    End Function
    Function RandInit(data() As Byte) As UInteger()
        Dim in1 As UInteger = BitConverter.ToUInt32(data, 0)
        Dim in2 As UInteger = BitConverter.ToUInt32(data, 4)
        Dim in3 As UInteger = BitConverter.ToUInt32(data, 8)
        Dim in4 As UInteger = BitConverter.ToUInt32(data, 12)
        Dim cond As Boolean = (in1 Or in2 Or in3 Or in4) <> 0

        Return New UInteger() {
                            If(cond, in1, &H1UI),
                            If(cond, in2, &H6C078967UI),
                            If(cond, in3, &H714ACB41UI),
                            If(cond, in4, &H48077044UI)}
    End Function
    Function RandGen(ByRef rand_state() As UInteger) As UInteger
        Dim n As UInteger = rand_state(0) Xor (rand_state(0) << 11)
        rand_state(0) = rand_state(1)
        n = n Xor ((n >> 8) Xor rand_state(3) Xor (rand_state(3) >> 19))
        rand_state(1) = rand_state(2)
        rand_state(2) = rand_state(3)
        rand_state(3) = n
        Return n
    End Function
    Function GenKey(ByRef rand() As UInteger) As UInteger()
        Dim outKey() As UInteger = New UInteger(3) {0, 0, 0, 0}
        Dim idx1, idx2, idx3, idx4 As UInteger
        For i As Integer = 0 To STATE_SIZE - 1
            For j As Integer = 0 To NUM_ROUNDS - 1
                outKey(i) <<= 8
                idx1 = RandGen(rand) >> 26
                idx2 = RandGen(rand) >> 27
                idx3 = idx2 And 24
                idx4 = Course_key_table(idx1) >> CInt(idx3)
                outKey(i) = outKey(i) Or idx4 And &HFF
            Next
        Next
        Return outKey
    End Function
    Function GetRandKey(data() As Byte) As Byte()
        Dim randState() As UInteger = RandInit(data)
        Dim keyState() As UInteger = GenKey(randState)
        Dim key() As Byte = BitConverter.GetBytes(keyState(0)) _
        .Concat(BitConverter.GetBytes(keyState(1))) _
        .Concat(BitConverter.GetBytes(keyState(2))) _
        .Concat(BitConverter.GetBytes(keyState(3))).ToArray()
        Return key
    End Function
    Enum AESMODE
        ECB = 0
        CBC = 1
    End Enum
    Enum AESPadding
        ZERO = 0
        PKCS7 = 1
        NONE = 2
    End Enum
    Function AesEncrypt(buf() As Byte, key() As Byte, iv() As Byte, Optional mode As AESMODE = AESMODE.CBC, Optional padding As AESPadding = AESPadding.PKCS7) As Byte()
        If mode = AESMODE.CBC Then
            If iv Is Nothing Then
                Return Nothing
            End If
            If iv.Length < 16 Then
                Return Nothing
            End If
        End If

        Dim keysize As Integer = key.Length * 8
        If ((keysize <> 128) And (keysize <> 192) And (keysize <> 256)) Then
            Return Nothing
        End If

        Dim ek() As UInteger = ExpandKey(key, keysize)
        Dim bin = New MemoryStream(buf)
        Dim bout = New MemoryStream()
        Dim block(15) As Byte
        Dim C As Integer
        Dim state(,) As Byte
        Dim cblock() As Byte = iv

        C = bin.Read(block, 0, 16)
        While C > 0
            Select Case mode
                Case AESMODE.ECB
                    state = LoadState(block)
                    EncryptBlock(state, ek, keysize)
                    block = DumpState(state)
                    bout.Write(block, 0, C)
                Case AESMODE.CBC
                    state = LoadState(block)
                    EncryptBlock(state, ek, keysize)
                    Dim pblock() As Byte = DumpState(state)
                    pblock = XorBytes(pblock, cblock)
                    cblock = block.Clone()
                    bout.Write(pblock, 0, C)
                Case Else
                    Return Nothing
            End Select
            C = bin.Read(block, 0, 16)
        End While
        Dim b1() As Byte = bout.ToArray()
        C = GetPadCount(b1, padding)
        Dim b2() As Byte
        ReDim b2(b1.Length - C - 1)
        Buffer.BlockCopy(b1, 0, b2, 0, b1.Length - C)
        Return b2
    End Function
    Function AesDecrypt(buf() As Byte, key() As Byte, iv() As Byte, Optional mode As AESMODE = AESMODE.CBC, Optional padding As AESPadding = AESPadding.PKCS7) As Byte()
        If mode = AESMODE.CBC Then
            If iv Is Nothing Then
                Return Nothing
            End If
            If iv.Length < 16 Then
                Return Nothing
            End If
        End If

        Dim keysize As Integer = key.Length * 8
        If ((keysize <> 128) And (keysize <> 192) And (keysize <> 256)) Then
            Return Nothing
        End If

        Dim ek() As UInteger = ExpandKey(key, keysize)
        Dim bin = New MemoryStream(buf)
        Dim bout = New MemoryStream()
        Dim block(15) As Byte
        Dim C As Integer
        Dim state(,) As Byte
        Dim cblock() As Byte = iv

        C = bin.Read(block, 0, 16)
        While C > 0
            Select Case mode
                Case AESMODE.ECB
                    state = LoadState(block)
                    DecryptBlock(state, ek, keysize)
                    block = DumpState(state)
                    bout.Write(block, 0, C)
                Case AESMODE.CBC
                    state = LoadState(block)
                    DecryptBlock(state, ek, keysize)
                    Dim pblock() As Byte = DumpState(state)
                    pblock = XorBytes(pblock, cblock)
                    cblock = block.Clone()
                    bout.Write(pblock, 0, C)
                Case Else
                    Return Nothing
            End Select
            C = bin.Read(block, 0, 16)
        End While
        Dim b1() As Byte = bout.ToArray()
        C = GetPadCount(b1, padding)
        Dim b2() As Byte
        ReDim b2(b1.Length - C - 1)
        Buffer.BlockCopy(b1, 0, b2, 0, b1.Length - C)
        Return b2
    End Function
    Dim Invert As Boolean = True
    Private Sub DecryptBlock(state As Byte(,), key As UInteger(), keysize As Integer)
        Dim rounds As Integer
        Select Case keysize
            Case 128
                rounds = 10
            Case 192
                rounds = 12
            Case 256
                rounds = 14
            Case Else
                Return
        End Select
        AddRoundKey(state, GetUIntBlock(key, rounds))
        For i As Integer = 1 To rounds
            ShiftRows(state, Invert)
            SubBytes(state, Invert)
            AddRoundKey(state, GetUIntBlock(key, rounds - i))
            If i <> rounds Then MixColumns(state, Invert)
        Next
    End Sub
    Private Function GetPadCount(buf As Byte(), Optional padding As AESPadding = AESPadding.PKCS7) As Integer
        If padding = AESPadding.NONE Then Return 0
        Dim c As Integer = 0
        Dim keepgoing As Boolean = True
        Dim i As Integer = (buf.Length - 1)
        While (i >= 0) AndAlso keepgoing
            Select Case padding
                Case AESPadding.PKCS7
                    If (buf(i) = buf(buf.Length - 1)) Then
                        c += 1
                    Else
                        keepgoing = False
                    End If
                Case AESPadding.ZERO
                    If buf(i) = 0 Then
                        c += 1
                    Else
                        keepgoing = False
                    End If
            End Select
            i -= 1
        End While
        Select Case padding
            Case AESPadding.PKCS7
                If c > buf(buf.Length - 1) Then Return buf(buf.Length - 1)
                If buf(buf.Length - 1) <> c Then Return 0
        End Select
        Return c
    End Function
    Private Function ExpandKey(key As Byte(), Optional keysize As Integer = 0) As UInteger()
        If keysize = 0 Then keysize = key.Length * 8
        Dim numWords, count, init As Integer
        Select Case keysize
            Case 128
                numWords = 44
                count = 4
                init = 4
            Case 192
                numWords = 52
                count = 6
                init = 6
            Case 256
                numWords = 60
                count = 4
                init = 8
            Case Else
                Return Nothing
        End Select
        Dim expandedKey() As UInteger
        ReDim expandedKey(numWords - 1)
        Dim iteration As Integer = 1
        Dim i As Integer
        For i = 0 To init - 1
            expandedKey(i) = GetWord(key, i * 4)
        Next
        Dim counter As Integer = 0

        For i = init To numWords - 1 Step count
            Dim tmp As UInteger = expandedKey(i - 1)

            If (keysize = 256) And ((counter Mod 2) = 1) Then
                tmp = SubstituteWord(tmp)
            Else
                tmp = KeyScheduleCore(tmp, iteration)
                iteration += 1
            End If
            counter += 1
            For j As Integer = 0 To count - 1
                If (i + j) >= numWords Then
                    Exit For
                Else
                    tmp = tmp Xor expandedKey(i - init + j)
                    expandedKey(i + j) = tmp
                End If
            Next
        Next

        Return expandedKey
    End Function

    Function GetWord(b() As Byte, Optional offset As Integer = 0) As UInteger
        Dim ret As UInteger = 0
        For i As Integer = 0 To 3
            ret <<= 8
            ret = ret Or b(i + offset)
        Next
        Return ret
    End Function
    Function SubstituteWord(word As UInteger) As UInteger
        Return CUInt(CUInt(SBOX(word And &HFFUI)) Or
            (CUInt(SBOX((word >> 8) And &HFFUI)) << &H8UI) Or
            (CUInt(SBOX((word >> 16) And &HFFUI)) << &H10UI) Or
            (CUInt(SBOX((word >> 24) And &HFFUI)) << &H18UI))
    End Function
    Function KeyScheduleCore(word As UInteger, iteration As Integer) As UInteger
        Dim wOut As UInteger = SubstituteWord(RotateByteLeft(word))
        Dim C As UInteger
        C = CalcRcon(CByte(iteration))
        C <<= 24
        wOut = wOut Xor C
        Return wOut
    End Function
    Function RotateByteLeft(x As UInteger) As UInteger
        Return (CUInt(x << 8) Or CUInt(x >> 24))
    End Function
    Function CalcRcon(bin As Byte) As Byte
        If bin = 0 Then
            Return &H8D
        End If
        Dim b1 As Byte = 1
        Dim b2 As Byte
        While bin <> 1
            b2 = b1 And &H80
            b1 <<= 1
            If b2 = &H80 Then
                b1 = b1 Xor &H1B
            End If
            bin -= 1
        End While
        Return b1
    End Function
    Function PadBuffer(buf As Byte(), padfrom As Integer, padto As Integer, Optional padding As AESPadding = AESPadding.PKCS7) As Byte()
        If (padto < buf.Length) Or ((padto - padfrom) > 255) Then
            Return buf
        End If
        Dim b() As Byte
        ReDim b(padto - 1)
        Buffer.BlockCopy(buf, 0, b, 0, padfrom)
        For i As Integer = padfrom To padto - 1
            Select Case padding
                '============有疑问，未校验结果=====================
                Case AESPadding.PKCS7
                    b(i) = CByte(padto - padfrom)
                Case AESPadding.ZERO
                Case AESPadding.NONE
                    b(i) = 0
                Case Else
                    Return buf
                    '==================================================
            End Select
        Next
        Return b
    End Function
    Function PadBuffer(buf As Byte(), blocksize As Integer, Optional padding As AESPadding = AESPadding.PKCS7) As Byte()
        Dim extraBlock As Integer = If((buf.Length Mod blocksize) = 0 AndAlso padding = AESPadding.NONE, 0, 1)
        Return PadBuffer(buf, buf.Length, ((buf.Length / blocksize) + extraBlock) * blocksize, padding)
    End Function
    Function LoadState(buf As Byte(), Optional offset As Integer = 0) As Byte(,)
        Dim state(3, 3) As Byte
        Dim c As Integer = 0
        For i As Integer = 0 To 3
            For j As Integer = 0 To 3
                state(j, i) = buf(c + (offset * 16))
                c += 1
            Next
        Next
        Return state
    End Function
    Function DumpState(state As Byte(,)) As Byte()
        Dim b(15) As Byte
        Dim c As Integer = 0
        For i As Integer = 0 To 3
            For j As Integer = 0 To 3
                b(c) = state(j, i)
                c += 1
            Next
        Next
        Return b
    End Function

    Function GetColumn(state As Byte(,), index As Integer) As Byte()
        Dim b(3) As Byte
        For i As Integer = 0 To 3
            b(i) = state(i, index)
        Next
        Return b
    End Function

    Sub PutColumn(ByRef state As Byte(,), b As Byte(), index As Integer)
        For i As Integer = 0 To 3
            state(i, index) = b(i)
        Next
    End Sub
    Sub EncryptBlock(ByRef state As Byte(,), key As UInteger(), keysize As Integer)
        Dim rounds As Integer
        Select Case keysize
            Case 128
                rounds = 10
            Case 192
                rounds = 12
            Case 256
                rounds = 14
            Case Else
                Return
        End Select
        AddRoundKey(state, GetUIntBlock(key))
        For i As Integer = 1 To rounds
            SubBytes(state)
            ShiftRows(state)
            If i <> rounds Then MixColumns(state)
            AddRoundKey(state, GetUIntBlock(key, i))
        Next
    End Sub
    Function XorBytes(b1 As Byte(), b2 As Byte()) As Byte()
        Dim rb() As Byte
        ReDim rb(b1.Length - 1)
        For i As Integer = 0 To b1.Length - 1
            rb(i) = CByte((b1(i) Xor b2(i Mod b2.Length)))
        Next
        Return rb
    End Function
    Function GetByteBlock(key() As UInteger, Optional offset As Integer = 0) As Byte()
        Return {CByte((key(offset) >> 24) And &HFF),
        CByte((key(offset) >> 16) And &HFF),
        CByte((key(offset) >> 8) And &HFF),
        CByte(key(offset) And &HFF)}
    End Function
    Function GetUIntBlock(key As UInteger(), Optional offset As Integer = 0) As UInteger()
        Dim tmp(3) As UInteger
        For i As Integer = 0 To 3
            tmp(i) = key(i + (offset * 4))
        Next
        Return tmp
    End Function

    Sub AddRoundKey(ByRef state As Byte(,), key As UInteger())
        For i As Integer = 0 To 3
            Dim kb = GetByteBlock(key, i)
            For j As Integer = 0 To 3
                state(j, i) = state(j, i) Xor kb(j)
            Next
        Next
    End Sub
    Sub SubBytes(ByRef state As Byte(,), Optional invert As Boolean = False)
        Dim sb() As Byte
        If invert Then
            sb = iSBOX
        Else
            sb = SBOX
        End If

        For i As Integer = 0 To 3
            For j As Integer = 0 To 3
                state(j, i) = sb(state(j, i))
            Next
        Next
    End Sub

    Sub ShiftRows(ByRef state As Byte(,))
        For j As Integer = 0 To 3
            For i As Integer = 0 To j - 1
                Dim b As Byte = state(j, 0)
                For c As Integer = 1 To 3
                    state(j, c - 1) = state(j, c)
                Next
                state(j, 3) = b
            Next
        Next
    End Sub

    Sub InvShiftRows(ByRef state As Byte(,))
        For j As Integer = 0 To 3
            For i As Integer = 0 To j - 1
                Dim b As Byte = state(j, 3)
                For c As Integer = 3 To 0 + 1 Step -1
                    state(j, c) = state(j, c - 1)
                Next
                state(j, 0) = b
            Next
        Next
    End Sub

    Sub ShiftRows(ByRef state As Byte(,), Optional invert As Boolean = False)
        If invert Then
            InvShiftRows(state)
        Else
            ShiftRows(state)
        End If
    End Sub
    Function Gmul(a As Byte, b As Byte) As Byte
        Dim p As Byte = 0
        Dim hibit As Byte
        For i As Integer = 0 To 7
            If (b And &H1) = &H1 Then
                p = p Xor a
            End If
            hibit = CByte(a And &H80)
            a <<= 1
            If hibit = &H80 Then
                a = a Xor &H1B
            End If
            b >>= 1
        Next
        Return p
    End Function
    Sub MixColumn(ByRef r As Byte())
        Dim a(3) As Byte
        For i As Integer = 0 To 3
            a(i) = r(i)
        Next
        r(0) = CByte(Gmul(a(0), 2) Xor a(3) Xor a(2) Xor Gmul(a(1), 3))
        r(1) = CByte(Gmul(a(1), 2) Xor a(0) Xor a(3) Xor Gmul(a(2), 3))
        r(2) = CByte(Gmul(a(2), 2) Xor a(1) Xor a(0) Xor Gmul(a(3), 3))
        r(3) = CByte(Gmul(a(3), 2) Xor a(2) Xor a(1) Xor Gmul(a(0), 3))
    End Sub

    Sub InvMixColumn(ByRef r As Byte())
        Dim a(3) As Byte
        For i As Integer = 0 To 3
            a(i) = r(i)
        Next
        r(0) = CByte(Gmul(a(0), 14) Xor Gmul(a(3), 9) Xor Gmul(a(2), 13) Xor Gmul(a(1), 11))
        r(1) = CByte(Gmul(a(1), 14) Xor Gmul(a(0), 9) Xor Gmul(a(3), 13) Xor Gmul(a(2), 11))
        r(2) = CByte(Gmul(a(2), 14) Xor Gmul(a(1), 9) Xor Gmul(a(0), 13) Xor Gmul(a(3), 11))
        r(3) = CByte(Gmul(a(3), 14) Xor Gmul(a(2), 9) Xor Gmul(a(1), 13) Xor Gmul(a(0), 11))
    End Sub

    Sub MixColumn(ByRef state As Byte(,), index As Integer, Optional invert As Boolean = False)
        Dim col() As Byte = GetColumn(state, index)
        If invert Then
            InvMixColumn(col)
        Else
            MixColumn(col)
        End If
        PutColumn(state, col, index)
    End Sub

    Sub MixColumns(ByRef state As Byte(,), Optional invert As Boolean = False)
        For i As Integer = 0 To 3
            MixColumn(state, i, invert)
        Next
    End Sub

    ReadOnly SBOX() As Byte = {&H63, &H7C, &H77, &H7B, &HF2, &H6B, &H6F, &HC5, &H30, &H1, &H67, &H2B, &HFE, &HD7, &HAB, &H76,
                        &HCA, &H82, &HC9, &H7D, &HFA, &H59, &H47, &HF0, &HAD, &HD4, &HA2, &HAF, &H9C, &HA4, &H72, &HC0,
                        &HB7, &HFD, &H93, &H26, &H36, &H3F, &HF7, &HCC, &H34, &HA5, &HE5, &HF1, &H71, &HD8, &H31, &H15,
                        &H4, &HC7, &H23, &HC3, &H18, &H96, &H5, &H9A, &H7, &H12, &H80, &HE2, &HEB, &H27, &HB2, &H75,
                        &H9, &H83, &H2C, &H1A, &H1B, &H6E, &H5A, &HA0, &H52, &H3B, &HD6, &HB3, &H29, &HE3, &H2F, &H84,
                        &H53, &HD1, &H0, &HED, &H20, &HFC, &HB1, &H5B, &H6A, &HCB, &HBE, &H39, &H4A, &H4C, &H58, &HCF,
                        &HD0, &HEF, &HAA, &HFB, &H43, &H4D, &H33, &H85, &H45, &HF9, &H2, &H7F, &H50, &H3C, &H9F, &HA8,
                        &H51, &HA3, &H40, &H8F, &H92, &H9D, &H38, &HF5, &HBC, &HB6, &HDA, &H21, &H10, &HFF, &HF3, &HD2,
                        &HCD, &HC, &H13, &HEC, &H5F, &H97, &H44, &H17, &HC4, &HA7, &H7E, &H3D, &H64, &H5D, &H19, &H73,
                        &H60, &H81, &H4F, &HDC, &H22, &H2A, &H90, &H88, &H46, &HEE, &HB8, &H14, &HDE, &H5E, &HB, &HDB,
                        &HE0, &H32, &H3A, &HA, &H49, &H6, &H24, &H5C, &HC2, &HD3, &HAC, &H62, &H91, &H95, &HE4, &H79,
                        &HE7, &HC8, &H37, &H6D, &H8D, &HD5, &H4E, &HA9, &H6C, &H56, &HF4, &HEA, &H65, &H7A, &HAE, &H8,
                        &HBA, &H78, &H25, &H2E, &H1C, &HA6, &HB4, &HC6, &HE8, &HDD, &H74, &H1F, &H4B, &HBD, &H8B, &H8A,
                        &H70, &H3E, &HB5, &H66, &H48, &H3, &HF6, &HE, &H61, &H35, &H57, &HB9, &H86, &HC1, &H1D, &H9E,
                        &HE1, &HF8, &H98, &H11, &H69, &HD9, &H8E, &H94, &H9B, &H1E, &H87, &HE9, &HCE, &H55, &H28, &HDF,
                        &H8C, &HA1, &H89, &HD, &HBF, &HE6, &H42, &H68, &H41, &H99, &H2D, &HF, &HB0, &H54, &HBB, &H16}

    ReadOnly iSBOX() As Byte = {&H52, &H9, &H6A, &HD5, &H30, &H36, &HA5, &H38, &HBF, &H40, &HA3, &H9E, &H81, &HF3, &HD7, &HFB,
                        &H7C, &HE3, &H39, &H82, &H9B, &H2F, &HFF, &H87, &H34, &H8E, &H43, &H44, &HC4, &HDE, &HE9, &HCB,
                        &H54, &H7B, &H94, &H32, &HA6, &HC2, &H23, &H3D, &HEE, &H4C, &H95, &HB, &H42, &HFA, &HC3, &H4E,
                        &H8, &H2E, &HA1, &H66, &H28, &HD9, &H24, &HB2, &H76, &H5B, &HA2, &H49, &H6D, &H8B, &HD1, &H25,
                        &H72, &HF8, &HF6, &H64, &H86, &H68, &H98, &H16, &HD4, &HA4, &H5C, &HCC, &H5D, &H65, &HB6, &H92,
                        &H6C, &H70, &H48, &H50, &HFD, &HED, &HB9, &HDA, &H5E, &H15, &H46, &H57, &HA7, &H8D, &H9D, &H84,
                        &H90, &HD8, &HAB, &H0, &H8C, &HBC, &HD3, &HA, &HF7, &HE4, &H58, &H5, &HB8, &HB3, &H45, &H6,
                        &HD0, &H2C, &H1E, &H8F, &HCA, &H3F, &HF, &H2, &HC1, &HAF, &HBD, &H3, &H1, &H13, &H8A, &H6B,
                        &H3A, &H91, &H11, &H41, &H4F, &H67, &HDC, &HEA, &H97, &HF2, &HCF, &HCE, &HF0, &HB4, &HE6, &H73,
                        &H96, &HAC, &H74, &H22, &HE7, &HAD, &H35, &H85, &HE2, &HF9, &H37, &HE8, &H1C, &H75, &HDF, &H6E,
                        &H47, &HF1, &H1A, &H71, &H1D, &H29, &HC5, &H89, &H6F, &HB7, &H62, &HE, &HAA, &H18, &HBE, &H1B,
                        &HFC, &H56, &H3E, &H4B, &HC6, &HD2, &H79, &H20, &H9A, &HDB, &HC0, &HFE, &H78, &HCD, &H5A, &HF4,
                        &H1F, &HDD, &HA8, &H33, &H88, &H7, &HC7, &H31, &HB1, &H12, &H10, &H59, &H27, &H80, &HEC, &H5F,
                        &H60, &H51, &H7F, &HA9, &H19, &HB5, &H4A, &HD, &H2D, &HE5, &H7A, &H9F, &H93, &HC9, &H9C, &HEF,
                        &HA0, &HE0, &H3B, &H4D, &HAE, &H2A, &HF5, &HB0, &HC8, &HEB, &HBB, &H3C, &H83, &H53, &H99, &H61,
                        &H17, &H2B, &H4, &H7E, &HBA, &H77, &HD6, &H26, &HE1, &H69, &H14, &H63, &H55, &H21, &HC, &H7D}
    Sub SubBytesMatrix(ByRef state As Byte(,), Optional invert As Boolean = False)
        Dim sb() As Byte
        If invert Then
            sb = iSBOX
        Else
            sb = SBOX
        End If
        For i As Integer = 0 To 3
            For j As Integer = 0 To 3
                state(j, i) = sb(state(j, i))
            Next
        Next
    End Sub
End Module

using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

public class GetInferenceFromModel : MonoBehaviour
{
    [Serializable]
    public struct Prediction
    {
        public int predictedValue;
        public float[] predicted;

        public void SetPrediction(Tensor t)
        {
            predicted = t.AsFloats();
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            Debug.Log($"Predicted {predictedValue}");
        }
    }

    public NNModel modelAsset;
    public Prediction prediction;

    private Model _runtimeModel;
    private IWorker _engine;

    private float[,] data_5 = new float[,]
    {
        { -122.32908630371094f, 94.41389465332031f, -19.326452255249023f, -2.1549625396728516f, 1.3787156343460083f, -8.44534969329834f, 4.586106300354004f, -0.34071439504623413f, 1.5908191204071045f, 3.596421480178833f, 1.2314491271972656f, -7.789007186889648f, -7.055183410644531f, -131.23146057128906f, 92.67807006835938f, -21.379243850708008f, -0.10642874985933304f, 2.7318503856658936f, -6.253641605377197f, 0.9002978801727295f, -3.2300989627838135f, -0.5866867899894714f, 6.913369655609131f, 0.9565050601959229f, -7.536477565765381f, -6.8110833168029785f, -99.9175033569336f, 99.44062805175781f, -10.801342010498047f, 27.294647216796875f, -3.115222454071045f, 3.2374274730682373f, 4.191788673400879f, 4.790149688720703f, 9.13473129272461f, 5.6842169761657715f, 2.476422071456909f, -2.6147470474243164f, -8.204145431518555f, -62.450584411621094f, 86.3177490234375f, -2.6981430053710938f, 31.793832778930664f, 5.489672660827637f, 8.896905899047852f, 12.647079467773438f, 11.124368667602539f, 15.91593074798584f, 7.347958564758301f, 2.2702765464782715f, -3.678008556365967f, -3.985029697418213f, -34.27920150756836f, 87.21992492675781f, 0.8063119053840637f, 18.469646453857422f, 16.985347747802734f, 13.522666931152344f, 18.397174835205078f, 14.271764755249023f, 14.816620826721191f, 0.07882429659366608f, 0.6639054417610168f, 2.0141000747680664f, 3.516477108001709f, -30.558956146240234f, 85.02616119384766f, 4.410152435302734f, 10.824140548706055f, 25.000316619873047f, 15.17342758178711f, 21.335124969482422f, 17.83370590209961f, 14.081642150878906f, -2.4809646606445312f, -2.997018337249756f, 0.899965226650238f, 2.711231231689453f, -67.25239562988281f, 72.39590454101562f, 16.466184616088867f, 13.906866073608398f, 33.468135833740234f, 15.874610900878906f, 17.60363006591797f, 17.380531311035156f, 13.353614807128906f, 4.861020088195801f, -0.21078652143478394f, 0.40151751041412354f, 3.4276864528656006f, -70.34540557861328f, 72.57759857177734f, 10.740409851074219f, 9.168573379516602f, 24.779491424560547f, 18.632549285888672f, 23.650272369384766f, 3.9032883644104004f, 1.1485121250152588f, 6.6164326667785645f, 2.376375675201416f, 7.813376426696777f, 2.4894981384277344f, -68.0621566772461f, 77.38008880615234f, 7.104137420654297f, 3.8775010108947754f, 16.744800567626953f, 21.047422409057617f, 33.20895767211914f, -4.7608489990234375f, -11.809703826904297f, 6.161976337432861f, 6.297746181488037f, 7.53977108001709f, 5.44349479675293f, -84.89122772216797f, 84.55744934082031f, 9.051355361938477f, 4.829495429992676f, 19.162456512451172f, 23.89944839477539f, 29.630985260009766f, -4.615495681762695f, -7.566708087921143f, 7.7462921142578125f, 7.140862464904785f, 2.4755349159240723f, 7.9322662353515625f },
        { -107.49297332763672f, 82.62245178222656f, 14.073495864868164f, 9.602113723754883f, 18.62863540649414f, 20.89993667602539f, 26.217552185058594f, -5.202334403991699f, -4.365378379821777f, 8.227230072021484f, 5.6070756912231445f, 1.2752516269683838f, 8.205406188964844f, -136.04486083984375f, 86.01322937011719f, 22.193103790283203f, 12.790359497070312f, 19.84836196899414f, 19.47391700744629f, 22.64162826538086f, 3.109598159790039f, 0.9841232895851135f, 5.665776252746582f, 3.9797208309173584f, 0.7720673084259033f, 2.1661832332611084f, -160.0233612060547f, 84.83010864257812f, 24.239192962646484f, 17.91402244567871f, 21.15570068359375f, 17.031646728515625f, 11.938392639160156f, 6.280963897705078f, 1.8793970346450806f, 4.289701461791992f, 4.261075019836426f, 0.9436148405075073f, 3.49783992767334f, -146.16061401367188f, 86.54793548583984f, 6.2820563316345215f, 22.72125816345215f, 14.296030044555664f, 10.831489562988281f, 0.5899350047111511f, -0.014732122421264648f, -1.9648938179016113f, 1.1790732145309448f, 3.146123170852661f, 5.753753662109375f, 13.606843948364258f, -123.05753326416016f, 82.14910888671875f, -10.568183898925781f, 28.864295959472656f, 8.140111923217773f, 6.720909595489502f, -5.126477241516113f, -0.2942010462284088f, -1.9694210290908813f, 2.895561933517456f, 1.5942994356155396f, 7.163320541381836f, 11.854792594909668f, -127.84095001220703f, 79.49468994140625f, -4.78433895111084f, 18.38570785522461f, 0.8299584984779358f, 10.462007522583008f, -9.828948020935059f, 6.600838661193848f, -0.9434833526611328f, 3.709867000579834f, 12.378072738647461f, 6.835024356842041f, 5.652742385864258f, -126.8074722290039f, 71.182861328125f, -3.2620882987976074f, 6.299866199493408f, -10.41209602355957f, 10.996711730957031f, -5.143527030944824f, 5.623014450073242f, -4.971306800842285f, 2.3364861011505127f, 20.054248809814453f, 4.372373580932617f, -4.038097381591797f, -90.97135162353516f, 74.18917846679688f, -13.638018608093262f, 2.8705945014953613f, -12.616233825683594f, 9.901830673217773f, 1.2083293199539185f, 6.4684648513793945f, -0.5727980136871338f, 0.15948998928070068f, 9.684109687805176f, -2.683417797088623f, -7.88642692565918f, -26.20061683654785f, 91.70872497558594f, -19.03378677368164f, 12.623929023742676f, -10.524076461791992f, 7.55898380279541f, 8.857526779174805f, 12.4948091506958f, 4.965305805206299f, -7.86247444152832f, -3.8979899883270264f, -3.4068236351013184f, -10.512887001037598f, -2.8050496578216553f, 91.45759582519531f, -10.270798683166504f, 21.230953216552734f, -2.881608486175537f, 9.464658737182617f, 10.074634552001953f, 12.228429794311523f, 6.177061080932617f, -1.2976292371749878f, -1.7668999433517456f, -4.621397495269775f, -8.631250381469727f },
        { -4.835204601287842f, 72.77995300292969f, 1.5845788717269897f, 19.853271484375f, 7.04621696472168f, 11.111907958984375f, 9.90383243560791f, 13.920999526977539f, 9.014890670776367f, 9.269916534423828f, 4.384093761444092f, -5.925745964050293f, -4.24114990234375f, -6.171993255615234f, 73.813720703125f, -2.840531826019287f, 17.40125274658203f, 13.407840728759766f, 10.103057861328125f, 15.728616714477539f, 14.971874237060547f, 9.714767456054688f, 7.917734146118164f, 5.711486339569092f, -2.1365058422088623f, -0.08990257978439331f, -17.456968307495117f, 79.91768646240234f, -6.701159477233887f, 13.720916748046875f, 10.248430252075195f, 7.361164093017578f, 17.963346481323242f, 10.452466011047363f, 12.783279418945312f, 7.049964904785156f, 8.228992462158203f, -1.1877812147140503f, -5.487643241882324f, -22.79977035522461f, 86.91107940673828f, -4.393773555755615f, 9.725078582763672f, 2.5064191818237305f, 6.778234481811523f, 19.928203582763672f, 10.007247924804688f, 11.22545051574707f, 2.5700221061706543f, 3.878377914428711f, -4.235188961029053f, -6.950361251831055f, -32.454185485839844f, 86.80927276611328f, -9.38591480255127f, 11.247451782226562f, -0.49976587295532227f, 8.467218399047852f, 19.111553192138672f, 11.067221641540527f, 3.5232882499694824f, -3.0150303840637207f, 0.16402733325958252f, -7.010329246520996f, -7.074560642242432f, -36.536563873291016f, 90.89447784423828f, -17.033775329589844f, 15.119037628173828f, 0.6778392791748047f, 8.626893997192383f, 14.604402542114258f, 15.343180656433105f, 7.104147911071777f, 0.5848733186721802f, 1.8338439464569092f, -4.770255088806152f, -6.043727397918701f, -35.330657958984375f, 93.68923950195312f, -21.210477828979492f, 17.386112213134766f, 1.1125006675720215f, 6.339273452758789f, 11.323305130004883f, 16.099105834960938f, 9.443143844604492f, 1.141210913658142f, -1.133310079574585f, -1.7927278280258179f, -6.707889556884766f, -30.31104278564453f, 91.25859069824219f, -25.7887020111084f, 15.521336555480957f, 2.7261598110198975f, 6.374618053436279f, 8.324520111083984f, 11.300437927246094f, 5.158730983734131f, 1.1677091121673584f, -4.2480621337890625f, -3.5637731552124023f, -7.133746147155762f, -34.53657150268555f, 99.01930236816406f, -28.558473587036133f, 17.065189361572266f, 2.0843734741210938f, 7.1127729415893555f, 1.8415961265563965f, 8.276140213012695f, 1.3178067207336426f, 4.116971969604492f, -3.1836302280426025f, -10.813684463500977f, -8.36526870727539f, -46.90609359741211f, 94.28727722167969f, -25.804767608642578f, 30.25478744506836f, 5.890900611877441f, 4.3028130531311035f, -2.9193389415740967f, 5.4094109535217285f, 3.9029698371887207f, 2.061514139175415f, 3.799391269683838f, -12.245369911193848f, -7.544872283935547f },
        { -64.72100830078125f, 84.75709533691406f, -24.876434326171875f, 29.605953216552734f, 13.07705307006836f, 5.305731773376465f, -6.763913154602051f, 2.739868402481079f, 5.833261966705322f, -3.957070827484131f, 6.5474443435668945f, -9.95669174194336f, -1.721652626991272f, -84.1681900024414f, 86.11027526855469f, -19.245433807373047f, 29.31220245361328f, 15.422016143798828f, 8.142854690551758f, -1.8849798440933228f, 3.8789384365081787f, 7.540314674377441f, -6.186934947967529f, 5.621439456939697f, -11.021951675415039f, 5.551723003387451f, -103.99356842041016f, 91.89390563964844f, -19.2048282623291f, 29.783042907714844f, 16.5733585357666f, 6.184817790985107f, 3.2805099487304688f, 2.7491278648376465f, 7.686761856079102f, -7.615412712097168f, 6.958907127380371f, -13.002288818359375f, 4.790640354156494f, -106.81591033935547f, 100.04124450683594f, -26.11139678955078f, 28.719284057617188f, 14.801920890808105f, 4.278892517089844f, 4.519248008728027f, 1.908262014389038f, 8.534819602966309f, -10.784704208374023f, 10.135331153869629f, -9.77251148223877f, 4.987529754638672f, -69.79116821289062f, 102.79725646972656f, -33.734771728515625f, 26.80866241455078f, 12.382766723632812f, 9.392998695373535f, 2.7581634521484375f, -4.646551132202148f, 0.3226553201675415f, -16.7406005859375f, -2.057784080505371f, -8.81310749053955f, 0.04265299439430237f, -14.41329288482666f, 81.40748596191406f, -25.19626235961914f, 39.967586517333984f, 11.449260711669922f, 10.133034706115723f, -4.704531192779541f, -10.40093994140625f, -0.32572460174560547f, -9.257415771484375f, -5.370871067047119f, -2.4687156677246094f, -1.8745083808898926f, 5.792499542236328f, 67.12692260742188f, -17.987796783447266f, 37.892303466796875f, 16.142589569091797f, 4.40020751953125f, -0.8653183579444885f, -10.607763290405273f, -2.036679744720459f, -5.632508754730225f, -7.252520561218262f, -3.255779266357422f, -0.3068404197692871f, -0.7993007898330688f, 56.06631851196289f, -4.189288139343262f, 30.008298873901367f, 17.970867156982422f, -1.303321123123169f, -0.6713142395019531f, -7.199416160583496f, -3.4369847774505615f, -6.606348037719727f, -6.209057807922363f, -7.189687728881836f, -0.13632294535636902f, -16.997583389282227f, 49.018898010253906f, 6.17569637298584f, 22.995468139648438f, 16.148656845092773f, 1.5031378269195557f, -2.3100638389587402f, -6.5775299072265625f, -0.9956806302070618f, -9.351841926574707f, -2.3803670406341553f, -5.494778156280518f, -0.5976218581199646f, -37.20706558227539f, 44.22083282470703f, 8.219038963317871f, 24.731502532958984f, 15.525909423828125f, 1.6860320568084717f, -1.6533102989196777f, -8.355724334716797f, -2.5396666526794434f, -9.101602554321289f, -4.984560012817383f, -2.6527929306030273f, 5.102773666381836f },
        { -57.973876953125f, 42.50318908691406f, 6.83005428314209f, 25.629623413085938f, 10.153806686401367f, -0.8183934688568115f, 5.902078151702881f, -2.049466133117676f, -2.393618583679199f, -9.637697219848633f, -7.149927616119385f, -0.8833943605422974f, 8.116252899169922f, -66.59854888916016f, 47.15174865722656f, 1.036242961883545f, 21.441579818725586f, 9.801206588745117f, -2.151571273803711f, 6.657655715942383f, 2.09175705909729f, -4.700384140014648f, -12.618932723999023f, -5.969411373138428f, 4.863250732421875f, 9.617328643798828f, -83.85832977294922f, 54.64258575439453f, 3.8664536476135254f, 18.071565628051758f, 12.195606231689453f, -2.883331775665283f, 0.14944219589233398f, -1.6843955516815186f, -8.546210289001465f, -17.3756103515625f, -9.999860763549805f, 5.125391960144043f, 11.989681243896484f, -87.11434936523438f, 67.34798431396484f, -6.0520219802856445f, 6.139654159545898f, 12.007688522338867f, 0.9471181035041809f, -2.105750560760498f, -2.627537727355957f, -8.116569519042969f, -15.896371841430664f, -11.695898056030273f, 4.035645008087158f, 2.8548760414123535f, -78.58997344970703f, 85.55158996582031f, -17.937583923339844f, 6.507030487060547f, 17.50137710571289f, 8.013625144958496f, 5.19699764251709f, 7.516055107116699f, -4.15320348739624f, -11.67329216003418f, -9.109661102294922f, 4.163344383239746f, -6.21284294128418f, -108.66749572753906f, 89.22541809082031f, -3.5277628898620605f, 21.78749656677246f, 20.521678924560547f, 13.708925247192383f, 8.01312255859375f, 5.430106163024902f, -8.57933235168457f, -8.214438438415527f, -2.079554557800293f, 6.00433349609375f, -3.6120152473449707f, -147.38426208496094f, 81.68028259277344f, 10.447599411010742f, 33.13303756713867f, 12.950750350952148f, 10.60567855834961f, 7.677203178405762f, 2.043376922607422f, -9.077548027038574f, -7.582459449768066f, 2.123086929321289f, 5.211679935455322f, -1.0667104721069336f, -164.83819580078125f, 73.94085693359375f, 13.66942024230957f, 42.23102569580078f, 12.240808486938477f, 11.41059684753418f, 4.681209564208984f, 2.7192211151123047f, -7.831995964050293f, -5.550007343292236f, 2.7638797760009766f, 5.9723052978515625f, 1.718847632408142f, -164.14126586914062f, 51.1504020690918f, 19.081809997558594f, 38.2551155090332f, 12.48135757446289f, 12.574846267700195f, 8.42358684539795f, 0.022341668605804443f, -8.470386505126953f, -2.95068359375f, 5.81040620803833f, 3.8760457038879395f, 2.7161917686462402f, -109.06599426269531f, 28.97185516357422f, 5.681523323059082f, 15.214092254638672f, 14.706371307373047f, 17.5384521484375f, 4.824423789978027f, -2.2082653045654297f, -0.002796471118927002f, -4.243747711181641f, 5.551246166229248f, -3.9483089447021484f, -1.590318202972412f },
        { -31.32500457763672f, 28.68429946899414f, -0.35458308458328247f, 1.4204320907592773f, 12.665229797363281f, 12.250524520874023f, 4.9584431648254395f, 1.7906025648117065f, 2.7606008052825928f, -7.057924270629883f, 1.396209955215454f, -4.4969096183776855f, -3.2258522510528564f, 14.690973281860352f, 40.0780029296875f, 7.663129806518555f, 2.0478382110595703f, 18.061626434326172f, 15.20107650756836f, 13.929633140563965f, 7.499283313751221f, 6.089637756347656f, -2.9498867988586426f, -5.432595252990723f, 0.32110559940338135f, -2.781557559967041f, 0.4174451529979706f, 42.12232208251953f, 19.6964054107666f, 8.717751502990723f, 25.13672637939453f, 19.911785125732422f, 18.63592529296875f, 11.173727035522461f, 7.453043460845947f, 3.76811146736145f, -6.838873386383057f, 2.7284016609191895f, -1.2365930080413818f, -45.03136444091797f, 57.92451477050781f, 28.885910034179688f, 15.552637100219727f, 26.663429260253906f, 16.36265754699707f, 18.662254333496094f, 11.259716033935547f, 8.617889404296875f, 7.337857723236084f, -1.2489666938781738f, 4.739038944244385f, -1.5059460401535034f, -77.64004516601562f, 72.92703247070312f, 25.890295028686523f, 19.0491943359375f, 29.3902530670166f, 13.120988845825195f, 14.77779769897461f, 3.3757920265197754f, 6.950335502624512f, -1.5641990900039673f, 3.5962600708007812f, 5.714821815490723f, 4.879790306091309f, -93.66433715820312f, 72.86605834960938f, 20.015785217285156f, 16.65765380859375f, 24.636123657226562f, 16.168935775756836f, 15.80459213256836f, 3.7401726245880127f, 8.792867660522461f, -3.34647274017334f, 1.2845079898834229f, 0.4574245512485504f, 6.800896644592285f, -107.01110076904297f, 77.83634948730469f, 11.599264144897461f, 16.38513946533203f, 19.796619415283203f, 12.753069877624512f, 14.327966690063477f, 5.6217193603515625f, 8.743023872375488f, -1.9819618463516235f, -0.08584964275360107f, 3.04259991645813f, 8.724737167358398f, -107.76505279541016f, 92.95996856689453f, 7.294796943664551f, 16.105215072631836f, 16.149038314819336f, 1.4804449081420898f, 3.4268503189086914f, 1.9438008069992065f, 4.813182830810547f, 0.24666345119476318f, 4.884311676025391f, 4.009210586547852f, 7.002175807952881f, -114.8740005493164f, 95.87271118164062f, -0.9256129264831543f, 24.257028579711914f, 17.852693557739258f, -1.1584396362304688f, -0.6621229648590088f, 1.3828080892562866f, 3.366503953933716f, 1.1832102537155151f, 3.149966239929199f, 7.527636528015137f, 3.9202098846435547f, -101.66156005859375f, 76.37101745605469f, -15.475475311279297f, 36.20256805419922f, 27.49626922607422f, -2.5285849571228027f, 3.7895865440368652f, -1.4122755527496338f, 5.84797477722168f, -1.0731661319732666f, -6.381987571716309f, 8.276473045349121f, -1.2073230743408203f },
        { -104.09510040283203f, 78.9406967163086f, -16.723079681396484f, 32.332237243652344f, 18.467533111572266f, -8.774714469909668f, 4.520977020263672f, -3.194314956665039f, 4.868673324584961f, -3.1538772583007812f, -6.601144313812256f, 5.011861324310303f, -0.07170534133911133f, -130.9971466064453f, 112.11866760253906f, -6.38754415512085f, 26.510173797607422f, 6.105534076690674f, -11.85133171081543f, -4.540898323059082f, -5.543166637420654f, 0.06072807312011719f, -1.8431485891342163f, -1.8144222497940063f, -7.756138801574707f, -0.9489506483078003f, -140.60928344726562f, 124.050537109375f, 7.604901313781738f, 27.19510269165039f, 1.154488205909729f, -16.38683319091797f, -11.979774475097656f, -5.292081832885742f, 8.433540344238281f, -2.182033061981201f, 3.317939281463623f, -17.767667770385742f, -1.2132545709609985f, -143.44041442871094f, 128.0181427001953f, 8.513948440551758f, 29.29131507873535f, -4.799624919891357f, -20.958162307739258f, -12.203350067138672f, -5.761251449584961f, 6.169369220733643f, -7.910454750061035f, 3.0002615451812744f, -21.636322021484375f, -3.2564773559570312f, -141.0702667236328f, 133.48736572265625f, 2.8097639083862305f, 23.849658966064453f, -8.323533058166504f, -19.641454696655273f, -8.683980941772461f, -8.232356071472168f, 4.794826507568359f, -11.27795124053955f, 1.463744044303894f, -17.0631046295166f, -3.4014525413513184f, -135.77120971679688f, 136.58126831054688f, -6.688133239746094f, 17.49161720275879f, -7.061942100524902f, -16.612483978271484f, -9.55665111541748f, -9.139598846435547f, 6.570855617523193f, -12.536033630371094f, 0.8542122840881348f, -12.025835037231445f, -3.1156749725341797f, -57.08571243286133f, 108.07816314697266f, -20.43404197692871f, 24.63568878173828f, -5.414368152618408f, 2.7774465084075928f, -1.0450315475463867f, -3.1625771522521973f, -0.8537869453430176f, -9.352396011352539f, 4.73084831237793f, -0.9181283116340637f, -9.998882293701172f, -16.989395141601562f, 96.24687194824219f, -11.450626373291016f, 23.4847354888916f, 5.24928092956543f, 8.307028770446777f, 9.685307502746582f, -0.6306624412536621f, -0.4466022253036499f, -8.572687149047852f, 5.817597389221191f, -3.014319896697998f, -12.165375709533691f, -30.219802856445312f, 96.71408081054688f, 16.375104904174805f, 17.18860626220703f, 13.525550842285156f, 4.576668739318848f, 12.933584213256836f, 1.1363871097564697f, 6.544773101806641f, 0.5641194581985474f, 6.979671478271484f, -1.276699185371399f, -6.094626426696777f, -48.82966232299805f, 92.39021301269531f, 34.820621490478516f, 16.909812927246094f, 15.85999870300293f, 2.11523699760437f, 9.964431762695312f, 3.3645405769348145f, 10.353080749511719f, 1.9823626279830933f, 1.5870264768600464f, 1.0198637247085571f, -4.016855716705322f },
        { -63.38788986206055f, 90.18063354492188f, 37.02446746826172f, 22.72024154663086f, 18.26386260986328f, 1.470505714416504f, 5.6674394607543945f, 3.657735586166382f, 9.472488403320312f, 1.417503833770752f, 2.5478103160858154f, -2.7710328102111816f, -3.6196563243865967f, -76.73967742919922f, 94.13880920410156f, 35.40851593017578f, 22.295013427734375f, 14.42426872253418f, -3.9626102447509766f, 0.05940428376197815f, 1.6542021036148071f, 7.190370559692383f, -0.299005389213562f, -0.604472279548645f, -2.468722343444824f, -3.393824577331543f, -88.9365234375f, 102.45362854003906f, 39.14336395263672f, 19.051353454589844f, 7.406736373901367f, -5.722636699676514f, -2.161653757095337f, 1.478563904762268f, 4.066932678222656f, -1.9620052576065063f, -4.649235725402832f, -4.228973865509033f, -1.6172441244125366f, -115.07984924316406f, 100.80816650390625f, 33.398338317871094f, 20.502887725830078f, 6.132020950317383f, -1.966109275817871f, -1.2962052822113037f, 0.16176074743270874f, 2.1965441703796387f, -1.1551826000213623f, -5.474292278289795f, -9.087190628051758f, 0.5760032534599304f, -110.27359008789062f, 90.26243591308594f, 0.9320777654647827f, 45.12256622314453f, 17.99858856201172f, -8.294506072998047f, -4.249293327331543f, -2.1460015773773193f, 12.907543182373047f, 4.911162376403809f, -12.527064323425293f, -3.304293394088745f, 9.511137008666992f, -111.46849060058594f, 92.53719329833984f, -12.75783920288086f, 48.09339904785156f, 16.508852005004883f, -6.406706809997559f, -3.5271613597869873f, 0.011437177658081055f, 9.361937522888184f, 4.610771656036377f, -14.576041221618652f, -1.1520966291427612f, 8.02496337890625f, -151.78099060058594f, 99.65451049804688f, 1.8626519441604614f, 29.600662231445312f, 10.682167053222656f, 4.58315372467041f, -1.8923697471618652f, 3.0897045135498047f, 1.2590008974075317f, 2.2447307109832764f, -5.6684417724609375f, -5.170682907104492f, -3.54142427444458f, -171.26722717285156f, 113.80564880371094f, 18.782150268554688f, 18.68116569519043f, -0.02309533953666687f, -1.3345527648925781f, -7.697957992553711f, 5.469573974609375f, 3.924198627471924f, 4.256278038024902f, 0.21177145838737488f, -12.07645034790039f, -5.8700947761535645f, -174.7451629638672f, 122.51248931884766f, 21.66122055053711f, 19.110870361328125f, -6.209712028503418f, -8.999311447143555f, -12.152545928955078f, 7.6833696365356445f, 5.5478692054748535f, -0.0064357370138168335f, -0.5311402082443237f, -11.040090560913086f, -1.8991451263427734f, -173.39434814453125f, 131.38729858398438f, 29.39773178100586f, 22.15131378173828f, -15.276103973388672f, -14.020305633544922f, -11.493168830871582f, 6.482689380645752f, 6.80121374130249f, -2.128361225128174f, 0.4950331449508667f, -13.659252166748047f, 0.6500122547149658f },
        { -170.48800659179688f, 135.00135803222656f, 35.35169219970703f, 25.000093460083008f, -16.297088623046875f, -17.80449867248535f, -15.711978912353516f, 7.648713111877441f, 5.91152286529541f, -5.278026580810547f, 3.117237091064453f, -15.92509937286377f, 5.754144668579102f, -129.4906005859375f, 115.26481628417969f, 17.886886596679688f, 19.086742401123047f, -2.381852865219116f, -6.0275373458862305f, -12.366938591003418f, -0.5210274457931519f, 0.2926103472709656f, -1.6199580430984497f, -3.109513759613037f, -15.98751449584961f, 9.252889633178711f, -47.18730163574219f, 87.67433166503906f, -14.69306468963623f, 13.853200912475586f, 4.971315860748291f, -5.490603923797607f, 8.395061492919922f, 1.6498663425445557f, 3.396641969680786f, -5.288569450378418f, -11.657855987548828f, -16.26190185546875f, -1.7345877885818481f, 5.008236885070801f, 69.62763977050781f, -3.961383581161499f, 6.787508964538574f, 0.2026214599609375f, -4.626684665679932f, 12.682451248168945f, 4.119699478149414f, 5.545843124389648f, -5.356865882873535f, -4.793831825256348f, -12.80678653717041f, -2.779275894165039f, 2.794635772705078f, 52.506813049316406f, 26.04688835144043f, 5.211694717407227f, -3.692793369293213f, 0.46473777294158936f, 12.368640899658203f, 0.7789652347564697f, 2.7305994033813477f, -1.413576364517212f, -0.6316157579421997f, -6.742793560028076f, 0.5133563280105591f, -27.254301071166992f, 51.46829605102539f, 39.72100067138672f, 19.62042236328125f, 3.1391172409057617f, -6.357404708862305f, 8.038369178771973f, 4.771278381347656f, 5.472761154174805f, 7.884206771850586f, -1.8154836893081665f, -7.939159393310547f, -0.056727051734924316f, -69.00919342041016f, 79.16510009765625f, 18.526891708374023f, 25.834516525268555f, 12.170391082763672f, -13.886346817016602f, 5.532466888427734f, 7.377509593963623f, 13.13762378692627f, 12.585872650146484f, -9.084935188293457f, -11.522153854370117f, -0.6184630990028381f, -106.75275421142578f, 94.03645324707031f, 0.02009648084640503f, 32.21331024169922f, 14.538005828857422f, -11.210931777954102f, 7.602405071258545f, 1.7426286935806274f, 22.586566925048828f, 12.925067901611328f, -15.428424835205078f, -15.041159629821777f, -2.12343168258667f, -124.79068756103516f, 97.73492431640625f, -4.0256242752075195f, 33.78126907348633f, 17.49198341369629f, -8.30291748046875f, 8.55063247680664f, 6.937045097351074f, 22.704076766967773f, 11.617207527160645f, -19.95846176147461f, -19.219547271728516f, -2.334628105163574f, -137.05357360839844f, 105.10603332519531f, -7.357989311218262f, 33.512943267822266f, 10.657169342041016f, -7.600871562957764f, 10.491439819335938f, 7.7106757164001465f, 14.948525428771973f, 7.785541534423828f, -18.395936965942383f, -16.991483688354492f, 1.7241454124450684f },
        { -143.7374267578125f, 114.17257690429688f, -13.658233642578125f, 33.17328643798828f, 4.328927993774414f, -7.945330619812012f, 12.767133712768555f, 3.2971832752227783f, 12.059362411499023f, 10.393773078918457f, -18.736974716186523f, -16.363094329833984f, 8.216541290283203f, -123.84833526611328f, 94.97457885742188f, -15.450576782226562f, 38.09786605834961f, 12.708028793334961f, -16.045042037963867f, 11.554173469543457f, 2.5032777786254883f, 17.559059143066406f, 6.668635845184326f, -13.684301376342773f, -10.264833450317383f, -0.9592761993408203f, -106.19011688232422f, 80.32892608642578f, -11.555130004882812f, 38.84368133544922f, 18.21950340270996f, -16.356103897094727f, 9.80093002319336f, -0.37648552656173706f, 14.966957092285156f, 5.7176408767700195f, -11.327725410461426f, -6.2281084060668945f, -5.438170433044434f, -117.59333801269531f, 111.374267578125f, -8.71676254272461f, 31.795391082763672f, 10.68519115447998f, -20.72442626953125f, -3.104283571243286f, -9.83352279663086f, 11.484243392944336f, 8.52216911315918f, -9.959885597229004f, -10.352062225341797f, -1.2451629638671875f, -154.95973205566406f, 145.37640380859375f, -15.99228286743164f, 35.29054260253906f, 2.0579638481140137f, -16.02689552307129f, -11.89591121673584f, -11.798870086669922f, 9.673715591430664f, 6.49186372756958f, -8.832745552062988f, -14.752650260925293f, 5.956416130065918f, -181.66871643066406f, 152.40614318847656f, -19.4971866607666f, 39.05733871459961f, 0.9283382892608643f, -16.109846115112305f, -10.088462829589844f, -4.928770542144775f, 8.36790943145752f, 6.62166690826416f, -4.220376968383789f, -11.796440124511719f, 7.860210418701172f, -191.5340576171875f, 147.5258026123047f, -23.72994613647461f, 39.207733154296875f, 1.1597691774368286f, -14.896787643432617f, -8.604104995727539f, -2.9866528511047363f, 9.258602142333984f, 4.703299522399902f, -3.2823879718780518f, -7.141112804412842f, 7.819614410400391f, -179.81512451171875f, 127.471435546875f, -27.341400146484375f, 25.93031120300293f, 6.66392707824707f, -2.763970375061035f, -4.65471887588501f, -0.4518069326877594f, -2.5492262840270996f, 1.1546564102172852f, -3.1236958503723145f, 2.2654144763946533f, 4.876596927642822f, -73.57412719726562f, 86.95274353027344f, -9.350366592407227f, 30.65565299987793f, 1.6046088933944702f, 8.6856107711792f, -3.2295420169830322f, -4.995258331298828f, -11.01546859741211f, -10.100358963012695f, -5.813381195068359f, -3.6315035820007324f, -4.725595474243164f, -15.692591667175293f, 68.24612426757812f, -9.211362838745117f, 37.30952835083008f, 2.9867727756500244f, -4.754950523376465f, -3.19732666015625f, -5.308917045593262f, -8.377245903015137f, -11.889415740966797f, -4.884531021118164f, -4.415176868438721f, -6.954278945922852f },
        { -0.7478499412536621f, 49.55803680419922f, -8.005313873291016f, 42.87342834472656f, 7.648131370544434f, -14.781587600708008f, -4.940051078796387f, -11.242547988891602f, -9.931621551513672f, -14.259411811828613f, -8.069220542907715f, -4.566835403442383f, -4.280135154724121f, -9.507467269897461f, 40.47556686401367f, -1.5838569402694702f, 33.966087341308594f, 3.8609485626220703f, -15.855911254882812f, -0.9517543315887451f, -5.703512668609619f, -5.517327308654785f, -11.78980827331543f, -7.560694694519043f, -8.289323806762695f, -5.424661636352539f, -32.1064338684082f, 38.086883544921875f, 3.463859796524048f, 27.996292114257812f, 0.8593763709068298f, -15.143800735473633f, 3.26690673828125f, 1.8712517023086548f, -7.122060775756836f, -11.949197769165039f, -7.974773406982422f, -7.3547492027282715f, 0.9675378799438477f, -46.845619201660156f, 43.269744873046875f, -0.3469938635826111f, 25.975955963134766f, 3.2510523796081543f, -15.216127395629883f, 1.1981089115142822f, 2.54909610748291f, -9.432588577270508f, -9.650943756103516f, -9.768999099731445f, -5.812797546386719f, 2.7202091217041016f, -67.37956237792969f, 44.78272247314453f, -7.079673767089844f, 21.131168365478516f, -1.5867371559143066f, -14.437702178955078f, -1.1609861850738525f, 1.4713469743728638f, -7.837973594665527f, -10.54008960723877f, -7.78463077545166f, -4.422702789306641f, 1.3461594581604004f, -76.85383605957031f, 54.267364501953125f, -12.027767181396484f, 18.94341278076172f, -10.235363960266113f, -18.495616912841797f, -4.84503698348999f, 0.7921790480613708f, -7.284831523895264f, -11.800926208496094f, -7.43740177154541f, -1.6932915449142456f, -0.6570853590965271f, -77.52120971679688f, 72.35880279541016f, -13.201082229614258f, 14.540452003479004f, -16.766605377197266f, -20.565959930419922f, -2.775789499282837f, 5.203492164611816f, -8.974649429321289f, -8.867328643798828f, -5.944377899169922f, 2.771670341491699f, 1.6067925691604614f, -60.56708908081055f, 94.47035217285156f, -29.915231704711914f, 10.214431762695312f, -13.789665222167969f, -11.958066940307617f, -2.7062785625457764f, 3.9757962226867676f, -10.756521224975586f, -5.167148113250732f, -2.268803358078003f, 5.137244701385498f, -0.4132049083709717f, -52.77187728881836f, 115.44674682617188f, -34.006431579589844f, 3.36051607131958f, -11.917682647705078f, -0.2558327615261078f, 0.6588184833526611f, 5.432484149932861f, -4.187318801879883f, -4.6697492599487305f, 1.793940782546997f, 6.226851940155029f, -0.7826908230781555f, -76.68026733398438f, 115.62373352050781f, -22.89732551574707f, 0.20152288675308228f, -23.195533752441406f, 7.274914741516113f, 9.007585525512695f, 6.820592880249023f, -1.138471007347107f, -5.599688529968262f, 3.0242791175842285f, 10.813894271850586f, 3.4761836528778076f },
        { -91.0599365234375f, 117.44441223144531f, -22.65583038330078f, 1.77936851978302f, -33.625667572021484f, 10.965645790100098f, 12.391595840454102f, 7.678362846374512f, -7.227939605712891f, -6.798194885253906f, 4.273746967315674f, 13.006662368774414f, 6.603469371795654f, -106.88461303710938f, 117.84323120117188f, -21.479270935058594f, 2.758335828781128f, -34.18938064575195f, 13.09934139251709f, 11.334630012512207f, 4.630476951599121f, -9.60404109954834f, -8.506221771240234f, 10.53918743133545f, 10.445606231689453f, 3.9410200119018555f, -119.75942993164062f, 109.93865966796875f, -14.662860870361328f, 7.4343414306640625f, -33.44641876220703f, 11.469764709472656f, 6.133526802062988f, -1.0456087589263916f, -8.549001693725586f, -8.74600601196289f, 5.302206993103027f, 9.138992309570312f, 4.465164661407471f, -116.88279724121094f, 96.44610595703125f, -16.23654556274414f, 11.729829788208008f, -23.815214157104492f, 18.783599853515625f, 7.379152297973633f, -6.339812755584717f, -4.312906265258789f, -8.361797332763672f, -1.5181431770324707f, 11.895387649536133f, 2.3078837394714355f, -56.380916595458984f, 92.72300720214844f, -39.048221588134766f, 12.96747875213623f, -3.815272331237793f, 12.839539527893066f, 9.377159118652344f, -2.654266834259033f, 0.17420506477355957f, -15.509315490722656f, -12.515399932861328f, 4.671350479125977f, -3.998385429382324f, -11.412067413330078f, 82.5413589477539f, -29.45781135559082f, 6.7079925537109375f, 5.686469554901123f, 6.536574363708496f, 13.909093856811523f, -2.2159626483917236f, 2.2614004611968994f, -6.053299427032471f, -13.381675720214844f, 1.2943806648254395f, -5.298157691955566f, -12.021580696105957f, 57.710174560546875f, -1.860060214996338f, 0.8694696426391602f, 8.518601417541504f, 10.793318748474121f, 14.835826873779297f, -0.18181663751602173f, 2.669257164001465f, 6.213541030883789f, -6.797147750854492f, 6.759350776672363f, -2.8818352222442627f, -36.397377014160156f, 55.78459930419922f, 13.321113586425781f, -1.7689411640167236f, 9.03724193572998f, 18.12555694580078f, 17.20413589477539f, -6.333039283752441f, -4.377901077270508f, 6.072279930114746f, -0.739729642868042f, 11.599681854248047f, -6.213717937469482f, -68.72725677490234f, 71.50825500488281f, 8.30422306060791f, -4.705272197723389f, 5.015720367431641f, 17.91543960571289f, 18.96114730834961f, -7.314556121826172f, -1.3649927377700806f, 2.549574375152588f, 0.568291187286377f, 4.69043493270874f, -4.329373836517334f, -85.32353210449219f, 68.92854309082031f, 0.1115768551826477f, 2.013075828552246f, 3.22709321975708f, 15.232406616210938f, 19.95817756652832f, -4.0962934494018555f, 3.3182404041290283f, 0.7827075719833374f, -0.43847739696502686f, 2.0252795219421387f, 0.0921480655670166f },
        { -91.39324951171875f, 65.56838989257812f, -10.203398704528809f, 8.756110191345215f, 1.6095986366271973f, 8.372915267944336f, 6.743103981018066f, -7.433337211608887f, 5.853492736816406f, 0.4244704842567444f, -2.001011371612549f, -0.8782358169555664f, -0.10829424858093262f, -92.51890563964844f, 61.29863739013672f, -25.681060791015625f, 12.882608413696289f, 0.3480874300003052f, 5.181619167327881f, -2.7567081451416016f, -10.695053100585938f, 9.01270866394043f, 2.391366720199585f, -4.614718914031982f, -6.952895641326904f, -3.648738145828247f, -75.89251708984375f, 78.19361877441406f, -30.869522094726562f, 14.012191772460938f, -0.9839369058609009f, 8.57740592956543f, -0.1727500557899475f, -4.975253105163574f, 7.6864142417907715f, -0.4739508628845215f, -1.9770352840423584f, -6.0987443923950195f, -4.9193596839904785f, -47.13629150390625f, 93.5364761352539f, -39.17964172363281f, 11.325569152832031f, -8.33703327178955f, 3.037648916244507f, 9.547660827636719f, -1.7475063800811768f, 5.8888397216796875f, -4.487649917602539f, -0.9973108768463135f, -2.966620922088623f, -6.112821102142334f, -45.47310256958008f, 99.80656433105469f, -40.82080841064453f, 12.215045928955078f, -10.82900619506836f, 2.2159955501556396f, 12.155290603637695f, -1.7301267385482788f, 5.020194053649902f, -5.619379997253418f, 1.409986972808838f, -2.2181715965270996f, -5.730267524719238f, -69.38544464111328f, 101.46817016601562f, -32.86015701293945f, 20.72798728942871f, -12.473806381225586f, 6.169897079467773f, 5.486127853393555f, -1.7914882898330688f, 2.292914628982544f, -1.0933287143707275f, 9.90075397491455f, 0.19685962796211243f, -4.184340476989746f, -81.94409942626953f, 100.89338684082031f, -29.8984317779541f, 21.37212371826172f, -17.05300521850586f, 7.456392288208008f, -0.9117245674133301f, -2.2111988067626953f, -2.405729293823242f, -0.5764222741127014f, 10.061007499694824f, 2.717095375061035f, -3.231886863708496f, -99.75507354736328f, 103.06621551513672f, -21.504867553710938f, 20.828018188476562f, -24.8692626953125f, 12.699506759643555f, 1.065173625946045f, 3.6512975692749023f, -0.540874719619751f, 1.0478837490081787f, 10.580257415771484f, -1.013474464416504f, -6.307510852813721f, -110.4420394897461f, 101.29200744628906f, -20.457765579223633f, 25.235727310180664f, -27.418270111083984f, 15.628511428833008f, -1.0430516004562378f, 4.850147247314453f, 1.6982700824737549f, 4.601651668548584f, 11.682632446289062f, -5.181275367736816f, -5.860520362854004f, -108.75682830810547f, 100.8625717163086f, -27.555309295654297f, 25.013534545898438f, -25.073156356811523f, 20.227046966552734f, 2.7193057537078857f, 5.453568458557129f, 0.3232079744338989f, 0.9642269611358643f, 10.758310317993164f, -0.3291327953338623f, 0.8765988349914551f }
    };


    void Start()
    {
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.GPU);
        prediction = new Prediction();
    }

    public void MakePrediction()
    {
        TensorShape tensorShape = new TensorShape(1, 130, 13, 1);

        var inputX = new Tensor(tensorShape, data_5);
        Tensor outputY = _engine.Execute(inputX).PeekOutput();
        prediction.SetPrediction(outputY);
        inputX.Dispose();
    }

    private void OnDestroy()
    {
        _engine?.Dispose();
    }
}
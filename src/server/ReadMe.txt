1��Buffer ������bytes ��encoding/binary
�Ի������͵Ķ�д����

2��Codec �����˱��������� �����ӿ�

3��Degault  (ʵ����Codec�еı���뷽��)
������ DefaultSocketModel
���б��뷽����
�� ��DefaultSocketModel��������[]byte
���뷽�� 
�� ��[]byte���DefaultSocketModel
����һ��DefaultHandler ��Handler������������
SessionOpen  SessionClose MessageReceived

4��Session
Session�Ľṹ
CreateSession����
Close����
SetAttribute����
RemoveAttribute����
Write����
Read����

5��ServerSocket
Handler�ӿ�
ServerSocket�ṹ��
func CreateServer() *ServerSocket {����������
func (server *ServerSocket) Start(port int) {�������������������� һ���н���ͽ���clentConnection����
func clentConnection(session *Session, server *ServerSocket) {�ͻ������Ӵ���

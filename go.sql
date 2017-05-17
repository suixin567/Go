/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50624
Source Host           : localhost:3306
Source Database       : go

Target Server Type    : MYSQL
Target Server Version : 50624
File Encoding         : 65001

Date: 2017-01-18 19:02:49
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for item
-- ----------------------------
DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `itemtype` tinyint(20) DEFAULT '0',
  `sprite` char(255) DEFAULT NULL,
  `quality` char(20) DEFAULT NULL,
  `capacity` tinyint(2) DEFAULT NULL,
  `hp` tinyint(255) DEFAULT '0',
  `mp` tinyint(255) DEFAULT '0',
  `attack` tinyint(4) DEFAULT '0',
  `def` tinyint(4) DEFAULT '0',
  `speed` tinyint(4) DEFAULT '0',
  `sellprice` int(255) DEFAULT NULL,
  `buyprice` int(255) DEFAULT NULL,
  `description` char(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6001 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of item
-- ----------------------------
INSERT INTO `item` VALUES ('1000', '小瓶红药', '1', 'icon-potion1', 'Common', '10', '10', '0', '0', '0', '0', '5', '10', '加血的小药瓶');
INSERT INTO `item` VALUES ('1001', '大瓶红药', '1', 'icon-potion2', 'Common', '10', '20', '0', '0', '0', '0', '10', '20', '加血的大药瓶');
INSERT INTO `item` VALUES ('2000', '狼牙', '0', 'icon-etcFang', 'common', '10', '0', '0', '0', '0', '0', '5', '10', '野狼的牙齿');
INSERT INTO `item` VALUES ('5000', '木剑', '5', 'rod-icon', 'Rare', '1', '0', '0', '51', '0', '0', '20', '50000', '开荒的木剑');
INSERT INTO `item` VALUES ('5001', '上衣', '6', 'armor2-icon', 'Common', '1', '100', '0', '11', '22', '0', '2', '2', '帅气却冷');
INSERT INTO `item` VALUES ('5002', '力量戒指', '7', 'icon-ring', 'Common', '1', '1', '1', '1', '1', '0', '1', '1', '经典的戒指');
INSERT INTO `item` VALUES ('6000', '治愈术', '8', 'skill-13', 'Common', '1', '0', '0', '0', '0', '0', '10', '20', '可以治愈自己的技能');

-- ----------------------------
-- Table structure for playerinfo
-- ----------------------------
DROP TABLE IF EXISTS `playerinfo`;
CREATE TABLE `playerinfo` (
  `pid` tinyint(10) NOT NULL AUTO_INCREMENT,
  `uid` varchar(255) DEFAULT NULL COMMENT '此角色属于哪个账号',
  `name` varchar(255) DEFAULT NULL COMMENT '是否在线',
  `job` tinyint(4) DEFAULT NULL,
  `level` tinyint(4) DEFAULT NULL,
  `gold` int(11) unsigned NOT NULL,
  `exp` int(11) DEFAULT NULL,
  `atk` int(11) DEFAULT NULL,
  `def` int(11) DEFAULT NULL,
  `hp` int(11) DEFAULT NULL,
  `maxhp` int(11) DEFAULT NULL,
  `pointx` float DEFAULT NULL,
  `pointy` float DEFAULT NULL,
  `pointz` float DEFAULT NULL,
  `rotationx` float DEFAULT NULL,
  `rotationy` float DEFAULT NULL,
  `rotationz` float DEFAULT NULL,
  `rotationw` float DEFAULT NULL,
  `map` int(11) DEFAULT NULL,
  `active` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`pid`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of playerinfo
-- ----------------------------
INSERT INTO `playerinfo` VALUES ('7', 'qq', 'qqqqq', '0', '0', '7629', '0', '115', '69', '460', '946', '-4.29738', '-0.00000161925', '1.30192', '0', '0.950096', '0', '-0.311959', '2', '0');
INSERT INTO `playerinfo` VALUES ('9', 'qq', 'qqqqq2', '0', '0', '0', '0', '5', '5', '14', '14', '-2.81357', '0.0799989', '-21.6901', '0', '0.955825', '0', '0.293935', '2', '0');
INSERT INTO `playerinfo` VALUES ('10', 'ww', 'ww1', '0', '0', '0', '0', '5', '5', '14', '14', '-3.01899', '0.08', '-12.3891', '0', '-0.336826', '0', '0.941567', '2', '0');
INSERT INTO `playerinfo` VALUES ('11', 'ww', 'qqqqqwww', '1', '0', '0', '0', '5', '5', '14', '14', '1', '2', '3', '0', '0', '0', '0', '2', '0');
INSERT INTO `playerinfo` VALUES ('12', 'ee', 'ee1', '0', '0', '0', '0', '5', '5', '14', '14', '1', '2', '3', '0', '0', '0', '0', '2', '0');
INSERT INTO `playerinfo` VALUES ('13', 'ee', 'ee2', '1', '0', '0', '0', '5', '5', '14', '14', '1', '2', '3', '0', '0', '0', '0', '2', '0');
INSERT INTO `playerinfo` VALUES ('14', 'ww', 'ww', '0', '0', '0', '0', '5', '5', '14', '14', '-1.62358', '0.0799988', '-23.9975', '0', '-0.651726', '0', '0.758454', '2', '0');
INSERT INTO `playerinfo` VALUES ('15', 'rr', 'rr1', '0', '0', '0', '0', '5', '5', '14', '14', '-1.07245', '0.0800009', '-5.31899', '0', '0.719003', '0', '0.695007', '2', '0');
INSERT INTO `playerinfo` VALUES ('16', 'rr', '1wewqe', '0', '0', '0', '0', '5', '5', '14', '14', '1', '2', '3', '0', '0', '0', '0', '2', '0');
INSERT INTO `playerinfo` VALUES ('17', 'aa', 'aa111', '0', '0', '0', '0', '5', '5', '14', '14', '-4.44922', '0.0800009', '-5.9555', '0', '0.99722', '0', '-0.0745141', '2', '0');
INSERT INTO `playerinfo` VALUES ('18', 'cc', 'cc', '0', '0', '0', '0', '5', '5', '14', '14', '3.69514', '0.0800011', '-4.26675', '0', '0.911146', '0', '0.412085', '2', '0');
INSERT INTO `playerinfo` VALUES ('19', 'gg', 'gg', '0', '0', '0', '0', '5', '5', '14', '14', '-4.20798', '0.0800012', '-3.03621', '0', '0.906739', '0', '-0.421693', '2', '0');
INSERT INTO `playerinfo` VALUES ('20', 'gg', 'gg2', '1', '0', '0', '0', '5', '5', '14', '14', '-4.01152', '0.0800011', '-3.82887', '0', '-0.143523', '0', '0.989647', '2', '0');
INSERT INTO `playerinfo` VALUES ('21', 'bb', 'bb', '0', '0', '0', '0', '5', '5', '14', '14', '1', '2', '3', '0', '0', '0', '0', '2', '0');
INSERT INTO `playerinfo` VALUES ('22', 'bb', 'bb2', '1', '0', '0', '0', '5', '5', '14', '14', '-4.52102', '0.0800013', '-2.33393', '0', '0.981334', '0', '0.192313', '2', '0');
INSERT INTO `playerinfo` VALUES ('23', 'nn', 'nn', '0', '0', '4984', '0', '5', '5', '14', '14', '-4.61486', '0.0800014', '-0.92069', '0', '-0.555508', '0', '0.831511', '2', '0');
INSERT INTO `playerinfo` VALUES ('24', 'vv', 'vv', '0', '0', '48985', '0', '51', '17', '765', '1151', '-3.33633', '0.0800004', '-8.00084', '0', '0.999937', '0', '-0.0111995', '2', '0');
INSERT INTO `playerinfo` VALUES ('25', 'pp', 'pp', '0', '0', '0', '0', '5', '5', '14', '14', '-2.82492', '0.0800008', '-5.76364', '0', '0.984546', '0', '0.175129', '2', '0');
INSERT INTO `playerinfo` VALUES ('26', 'pp', 'pp2', '1', '0', '0', '0', '5', '5', '14', '14', '-2.09911', '0.0799998', '-13.8618', '0', '0.929724', '0', '-0.368258', '2', '0');

-- ----------------------------
-- Table structure for playeritem
-- ----------------------------
DROP TABLE IF EXISTS `playeritem`;
CREATE TABLE `playeritem` (
  `playername` varchar(25) NOT NULL DEFAULT '',
  `items` varchar(10000) DEFAULT '',
  `equipments` varchar(2000) DEFAULT '' COMMENT '角色已穿戴的装备数据',
  `skills` varchar(4000) DEFAULT NULL,
  PRIMARY KEY (`playername`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of playeritem
-- ----------------------------
INSERT INTO `playeritem` VALUES ('bb2', '[]', '[]', '[]');
INSERT INTO `playeritem` VALUES ('nn', '[{\"Id\":5001,\"Name\":\"上衣\",\"ItemType\":6,\"Sprite\":\"armor2-icon\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":0,\"Mp\":0,\"Attack\":0,\"Def\":0,\"Speed\":0,\"SellPrice\":2,\"BuyPrice\":2,\"Description\":\"帅气却冷\"},{\"Id\":5002,\"Name\":\"力量戒指\",\"ItemType\":7,\"Sprite\":\"icon-ring\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":1,\"Mp\":1,\"Attack\":1,\"Def\":1,\"Speed\":0,\"SellPrice\":1,\"BuyPrice\":1,\"Description\":\"经典的戒指\"},{\"Id\":5001,\"Name\":\"上衣\",\"ItemType\":6,\"Sprite\":\"armor2-icon\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":0,\"Mp\":0,\"Attack\":0,\"Def\":0,\"Speed\":0,\"SellPrice\":2,\"BuyPrice\":2,\"Description\":\"帅气却冷\"},{\"Id\":5002,\"Name\":\"力量戒指\",\"ItemType\":7,\"Sprite\":\"icon-ring\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":1,\"Mp\":1,\"Attack\":1,\"Def\":1,\"Speed\":0,\"SellPrice\":1,\"BuyPrice\":1,\"Description\":\"经典的戒指\"},{\"Id\":2000,\"Name\":\"狼牙\",\"ItemType\":0,\"Sprite\":\"icon-etcFang\",\"Quality\":\"common\",\"Capacity\":10,\"Hp\":0,\"Mp\":0,\"Attack\":0,\"Def\":0,\"Speed\":0,\"SellPrice\":5,\"BuyPrice\":10,\"Description\":\"野狼的牙齿\"}]', '[]', '[]');
INSERT INTO `playeritem` VALUES ('pp', '[]', '[]', '[]');
INSERT INTO `playeritem` VALUES ('pp2', '[]', '[]', '[]');
INSERT INTO `playeritem` VALUES ('qqqqq', '[]', '[{\"Id\":5002,\"Name\":\"力量戒指\",\"ItemType\":7,\"Sprite\":\"icon-ring\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":1,\"Mp\":1,\"Attack\":1,\"Def\":1,\"Speed\":0,\"SellPrice\":1,\"BuyPrice\":1,\"Description\":\"经典的戒指\"},{\"Id\":5001,\"Name\":\"上衣\",\"ItemType\":6,\"Sprite\":\"armor2-icon\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":100,\"Mp\":0,\"Attack\":11,\"Def\":22,\"Speed\":0,\"SellPrice\":2,\"BuyPrice\":2,\"Description\":\"帅气却冷\"}]', '[{\"Id\":0,\"Name\":\"治愈术\",\"Icon\":\"skill-13\",\"Des\":\"治疗自己或队友\",\"ApplyType\":\"Buff\",\"ApplyProperty\":\"Hp\",\"ApplyValue\":15,\"ApplyTime\":20,\"Mp\":10,\"ColdTime\":30,\"Job\":0,\"Level\":5,\"ReleaseType\":\"self\",\"Distance\":0}]');
INSERT INTO `playeritem` VALUES ('vv', '[]', '[{\"Id\":5001,\"Name\":\"上衣\",\"ItemType\":6,\"Sprite\":\"armor2-icon\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":100,\"Mp\":0,\"Attack\":45,\"Def\":11,\"Speed\":0,\"SellPrice\":2,\"BuyPrice\":2,\"Description\":\"帅气却冷\"},{\"Id\":5002,\"Name\":\"力量戒指\",\"ItemType\":7,\"Sprite\":\"icon-ring\",\"Quality\":\"Common\",\"Capacity\":1,\"Hp\":1,\"Mp\":1,\"Attack\":1,\"Def\":1,\"Speed\":0,\"SellPrice\":1,\"BuyPrice\":1,\"Description\":\"经典的戒指\"}]', '[]');

-- ----------------------------
-- Table structure for skill
-- ----------------------------
DROP TABLE IF EXISTS `skill`;
CREATE TABLE `skill` (
  `id` int(255) unsigned NOT NULL,
  `name` char(25) NOT NULL,
  `icon` char(25) NOT NULL,
  `des` varchar(30) NOT NULL,
  `applytype` char(25) NOT NULL,
  `applyproperty` char(25) NOT NULL,
  `applyvalue` int(25) NOT NULL DEFAULT '0',
  `applytime` float(25,0) NOT NULL,
  `mp` tinyint(20) NOT NULL,
  `coldtime` float(20,0) NOT NULL,
  `job` tinyint(10) NOT NULL,
  `level` tinyint(5) NOT NULL,
  `releasetype` char(25) NOT NULL,
  `distance` float(25,0) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of skill
-- ----------------------------
INSERT INTO `skill` VALUES ('0', '治愈术', 'skill-13', '治疗自己或队友', 'Buff', 'Hp', '15', '20', '10', '30', '0', '5', 'self', '0');
INSERT INTO `skill` VALUES ('1', '火球术', 'skill-11', '发出小火球攻击敌人', 'xxx', 'Hp', '10', '0', '5', '3', '0', '1', 'enemy', '10');

-- ----------------------------
-- Table structure for userinfo
-- ----------------------------
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE `userinfo` (
  `uid` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT '用户序号',
  `username` varchar(64) DEFAULT NULL,
  `password` varchar(64) DEFAULT NULL,
  `playeramount` int(5) unsigned DEFAULT '0' COMMENT '此账号下有多少个角色',
  `player1` varchar(255) DEFAULT NULL,
  `player2` varchar(255) DEFAULT NULL,
  `online` tinyint(2) DEFAULT '0',
  `lasttime` datetime DEFAULT NULL,
  `createdtime` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of userinfo
-- ----------------------------
INSERT INTO `userinfo` VALUES ('32', 'qq', 'qq', '2', 'qqqqq', 'qqqqq2', '0', '2017-01-16 20:42:14', '2016-12-11 16:12:34');
INSERT INTO `userinfo` VALUES ('37', 'ww', 'ww', '2', 'ww1', 'ww', '0', '2017-01-10 19:42:15', '2016-12-12 20:20:20');
INSERT INTO `userinfo` VALUES ('38', 'ee', 'ee', '2', 'ee1', 'ee2', null, '2016-12-13 22:34:52', '2016-12-13 22:33:49');
INSERT INTO `userinfo` VALUES ('39', 'rr', 'rr', '2', 'rr1', '1wewqe', '0', '2017-01-12 19:40:11', '2016-12-13 22:35:42');
INSERT INTO `userinfo` VALUES ('40', 'rrqq', 'rrqq', '0', '', '', '0', '2016-12-14 23:44:49', '2016-12-14 23:44:49');
INSERT INTO `userinfo` VALUES ('41', 'trt', '11', '0', '', '', '1', '2016-12-14 23:59:08', '2016-12-14 23:58:44');
INSERT INTO `userinfo` VALUES ('42', 'aa', 'aa', '1', 'aa111', '', '0', '2017-01-12 18:12:15', '2016-12-18 21:58:09');
INSERT INTO `userinfo` VALUES ('43', 'cc', 'cc', '1', 'cc', '', '0', '2017-01-01 21:16:04', '2017-01-01 21:07:58');
INSERT INTO `userinfo` VALUES ('44', 'gg', 'gg', '2', 'gg', 'gg2', '0', '2017-01-02 10:17:12', '2017-01-02 09:56:14');
INSERT INTO `userinfo` VALUES ('45', 'bb', 'bb', '2', 'bb', 'bb2', '0', '2017-01-08 00:50:02', '2017-01-08 00:46:04');
INSERT INTO `userinfo` VALUES ('46', 'nn', 'nn', '1', 'nn', '', '1', '2017-01-08 14:34:54', '2017-01-08 14:30:30');
INSERT INTO `userinfo` VALUES ('47', 'vv', 'vv', '1', 'vv', '', '0', '2017-01-11 18:34:55', '2017-01-08 19:31:32');
INSERT INTO `userinfo` VALUES ('48', 'pp', 'pp', '2', 'pp', 'pp2', '0', '2017-01-11 18:24:07', '2017-01-11 18:20:30');

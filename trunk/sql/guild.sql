/*
Navicat MySQL Data Transfer

Source Server         : 254
Source Server Version : 1
Source Host           : 10.10.10.254:3306
Source Database       : GameXY

Target Server Type    : MYSQL
Target Server Version : 1
File Encoding         : 1

Date: 2017-11-14 16:10:28
*/

SET FOREIGN_KEY_CHECKS=0;

/** 
 * @brief ���ɱ�
 * */
DROP TABLE IF EXISTS `Guild`;	
CREATE TABLE `Guild`
(
	`GuildId`	    INT 		NOT NULL,/*** ����ID*/
	`GuildName`   	VARCHAR(60)	NOT NULL,/*** ������*/
	`Master`		BIGINT 		NOT NULL,/*** ����*/
	`MasterName`  	VARCHAR(60)	NOT NULL,/*** ������(δ����)*/
	`GuildVal`  	INT			NOT NULL,/*** ���ɻ���*/
	`CreatTime` 	BIGINT 		NOT NULL,/*** ����ʱ��*/
	`RequestList` 	BLOB 		NOT NULL,
	`RequestFlag`	INT			NOT NULL,/*** �Ƿ���Ҫ����*/
	`Require`		INT			NOT NULL,/*** ��������*/	
	PRIMARY KEY(guildId),
	UNIQUE KEY guildNameIdx   (guildName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/**
 * @brief ���ɳ�Ա��
 * */
 DROP TABLE IF EXISTS `GuildMember`;
CREATE TABLE `GuildMember`
(
	`GuildId`		INT 		NOT NULL,/*** ����ID(��� ���ɱ�)*/
	`RoleId`		BIGINT 		NOT NULL,/*** ��Ա����ID*/
	`RoleName`   	VARCHAR(60)	NOT NULL,/*** ��Ա��*/
	`Rolelevel`     TINYINT 	NOT NULL,/*** ��Ա�ȼ�*/
	`Job`			TINYINT 	NOT NULL,/*** ְλ*/
	`Contribution`	INT 		NOT NULL,/*** ����*/
	`TianTiVal`		INT 		NOT NULL,/*** ���ݻ���*/
	PRIMARY KEY(roleId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/**
 * @brief ������
 * */
 DROP TABLE IF EXISTS `GuildAssistant`;
CREATE TABLE `GuildAssistant`
(
	`AssistantId`		INT 		NOT NULL,
	`RoleName`   		VARCHAR(60)	NOT NULL,/*** ��Ա��*/
	`GuildId`			INT 		NOT NULL,/*** ����ID(��� ���ɱ�)*/
	`AssistantItem`		INT 		NOT NULL,/*** ���׵���*/
	`CrtCount`			INT 		NOT NULL,/*** ��ǰ��������*/
	`MaxCount`			INT 		NOT NULL,/*** ��ǰ��������*/
	`CatchNum`			INT 		NOT NULL,/*** ��ɫ�������յ��ľ�������*/
	`Donator` 			BLOB 		NOT NULL,/*** �������б�*/
	PRIMARY KEY(AssistantId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;



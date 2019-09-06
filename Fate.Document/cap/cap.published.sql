/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 80013
 Source Host           : localhost:3306
 Source Schema         : testcap

 Target Server Type    : MySQL
 Target Server Version : 80013
 File Encoding         : 65001

 Date: 16/07/2019 17:51:36
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
create table `cap.published`
(
   Id                   varchar(255) not null,
   Name                 varchar(200) not null comment '交换机名称',
   Content              longtext  comment '内容',
   Retries              int  comment '重试次数',
   Added                datetime comment '添加时间',
   ExpiresAt            datetime  comment '过期时间',
   StatusName           varchar(30)  comment '发送的状态',
   Version              varchar(50)  comment '版本',
   primary key (Id)
) ENGINE = InnoDB AUTO_INCREMENT = 147 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;

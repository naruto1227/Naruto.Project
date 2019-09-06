/*
 Navicat Premium Data Transfer

 Source Server         : 218.78.55.142主库
 Source Server Type    : MySQL
 Source Server Version : 80017
 Source Host           : 218.78.55.142:3306
 Source Schema         : CP.Infrastructure

 Target Server Type    : MySQL
 Target Server Version : 80017
 File Encoding         : 65001

 Date: 06/09/2019 09:57:41
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Log
-- ----------------------------
DROP TABLE IF EXISTS `Log`;
CREATE TABLE `Log`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '消息内容',
  `CreateDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;

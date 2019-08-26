DROP DATABASE ocelot ;
CREATE DATABASE ocelot  DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE ocelot;

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for OcelotConfiguration
-- ----------------------------
DROP TABLE IF EXISTS `OcelotConfiguration`;
CREATE TABLE `OcelotConfiguration`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ReRoutes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL comment  '路由信息',
	 `DynamicReRoutes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL comment  '自定义路由信息',
	 `Aggregates` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL comment  '请求聚合',
	  `GlobalConfiguration` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL comment  '全局配置',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;

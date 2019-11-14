
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for configurationendpoint
-- ----------------------------
DROP TABLE IF EXISTS `ConfigurationEndPoint`;
CREATE TABLE `ConfigurationEndPoint` (
  `Id` varchar(100) NOT NULL DEFAULT '',
  `Key` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT '' COMMENT '配置的key',
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT '配置的值',
  `EnvironmentType` int(2) DEFAULT '0' COMMENT '环境的类型(0 测试 1 预发 2 正式)',
  `Group` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT '' COMMENT '配置所属的组名',
  `Remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT '配置的备注',
  `CreateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

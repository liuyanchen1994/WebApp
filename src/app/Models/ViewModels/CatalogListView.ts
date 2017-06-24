
import { Model } from 'app/Models/Model';

export interface CatalogListView extends Model {

	/**
	 * 
	 */
    name: string;

	/**
	 * 分类值
	 */
    value: string;

	/**
	 * 分类的类别
	 */
    type: string;

	/**
	 * 是否为顶级分类
	 */
    isTop: number;

	/**
	 * 上级/顶级(如果存在)分类
	 */
    topCatalog: string;

}

